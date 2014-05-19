package remoting;
import akka.actor.*;
import com.typesafe.config.Config;
import com.typesafe.config.ConfigFactory;

import akka.event.Logging;
import akka.event.LoggingAdapter;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;

class EchoActor extends UntypedActor {
	  LoggingAdapter log = Logging.getLogger(getContext().system(), this);
      HashMap<String,ActorRef> actorList = new HashMap<String,ActorRef>();
	  public void onReceive(Object message) throws Exception {
        if(!actorList.containsKey(getSender().toString())){
            actorList.put(getSender().toString(),getSender());
        }

	    if (message instanceof String){
            for(ActorRef ref : actorList.values()){
                if(!ref.toString().equals(getSender().toString())){
                    ref.tell(message, getSender());
                }
            }
            log.info("Message: "+ message);
        }

	  }
	}

public class Server {

	public static void main(String[] args) {
		Config config = ConfigFactory.load();
		ActorSystem sys = ActorSystem.create("RemoteSystem", config.getConfig("server"));
		sys.actorOf(Props.create(EchoActor.class), "EchoActor");
	}

}
