namespace Messages;
public class Greeting {
    static int messageNumber = 0;
    public string Message { get; set; }
    public Greeting() {
        Message = $"Greeting {messageNumber++} from {Environment.MachineName} at {DateTimeOffset.Now}";
    }
}