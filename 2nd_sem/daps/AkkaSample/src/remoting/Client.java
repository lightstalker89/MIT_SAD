package remoting;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

import akka.actor.ActorRef;
import akka.actor.ActorSelection;
import akka.actor.ActorSystem;
import akka.actor.Props;
import akka.actor.UntypedActor;
import akka.event.Logging;
import akka.event.LoggingAdapter;


import com.typesafe.config.Config;
import com.typesafe.config.ConfigFactory;

public class Client {
	public static void main(String[] args) throws IOException {
		Config config = ConfigFactory.load();
		
		ActorSystem sys = ActorSystem.create("RemoteSystem", config.getConfig("client"));
		ActorRef ref = sys.actorOf(Props.create(ClientSystem.class), "ClientSystem");
		
		ActorSelection actorRef = sys.actorSelection("akka.tcp://RemoteSystem@10.13.50.246:48120/user/EchoActor");
		
		//string line = string.Empty;
		String line = "";
		BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
		
		//InputStream instream = System.in;
		
		while(line != "end")
		{
	        System.out.print("Enter String:\n");
	        line = br.readLine();
	        
			actorRef.tell(line, ref);
		}
	}
}

class ClientSystem extends UntypedActor {
	
	  LoggingAdapter log = Logging.getLogger(getContext().system(), this);
	 
	  public void onReceive(Object message) throws Exception {
	    if (message instanceof String)
	      log.info("Hello " + message);
	  }
	}
