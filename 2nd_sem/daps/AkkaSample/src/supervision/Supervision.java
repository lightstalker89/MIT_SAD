package supervision;

import java.util.Random;

import static akka.pattern.Patterns.ask;
import scala.concurrent.Await;
import scala.concurrent.duration.Duration;
import akka.actor.ActorRef;
import akka.actor.ActorSystem;
import akka.actor.SupervisorStrategy;
import static akka.actor.SupervisorStrategy.resume;
import static akka.actor.SupervisorStrategy.restart;
import static akka.actor.SupervisorStrategy.stop;
import static akka.actor.SupervisorStrategy.escalate;
import akka.actor.SupervisorStrategy.Directive;
import akka.actor.OneForOneStrategy;
import akka.actor.Props;
import akka.actor.UntypedActor;
import akka.event.Logging;
import akka.event.LoggingAdapter;
import akka.japi.Function;

class Supervisor extends UntypedActor {
	
	LoggingAdapter log;
	ActorRef child;
	
	public Supervisor()
	{
		log = Logging.getLogger(getContext().system(), this);
		child = (ActorRef) getContext().actorOf(Props.create(Child.class));
	}
	

  private static SupervisorStrategy strategy =
    new OneForOneStrategy(10, Duration.create("1 minute"),
      new Function<Throwable, Directive>() {
        @Override
        public Directive apply(Throwable t) {
          if (t instanceof ArithmeticException) {
            return resume();
          } else if (t instanceof NullPointerException) {
            return restart();
          } else if (t instanceof IllegalArgumentException) {
            return stop();
          } else {
            return escalate();
          }
        }
      });
 
  @Override
  public SupervisorStrategy supervisorStrategy() {
    return strategy;
  }
 
 
  public void onReceive(Object o) {
	  log.info("Forwarding request to child " + o);
	  child.tell(o, getSender());
  }
}

class Child extends UntypedActor {
	  int state = 0;
	  
	  LoggingAdapter log = Logging.getLogger(getContext().system(), this);	  
	 
	  public void onReceive(Object o) throws Exception {
	    	if ((new Random()).nextInt(2) == 1) {
	      	  throw new NullPointerException("ouch");
	      	}
	    	
	    	log.info("Done processing: " + o);
	  }
}

public class Supervision {

	public static void main(String[] args) throws Exception {
		ActorSystem system = ActorSystem.create("MySystem");
		Props superprops = Props.create(Supervisor.class);
		ActorRef supervisor = system.actorOf(superprops, "supervisor");
		
		for (int i=0; i < 10; i++) supervisor.tell(i, ActorRef.noSender());
	}

}
