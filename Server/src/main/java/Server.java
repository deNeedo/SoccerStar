import java.io.IOException;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;

public class Server {
    public static void main(String[] args) {
        ConfigManager.getPreferences();
        TaskScheduler.startDailyTask(new DailyReset());
        try {
            // ServerSocket backlog requires further testing
            ServerSocket serverSocket = new ServerSocket(ConfigManager.getPORT(), 0, InetAddress.getByName(ConfigManager.getIP()));
            System.out.println("Server started on " + ConfigManager.getIP() + ":" + ConfigManager.getPORT());
            while (true) {
                Socket clientSocket = serverSocket.accept();
                new Thread(new ClientHandler(clientSocket)).start();
            }
        } catch (IOException e) {
            System.err.println("Server error: " + e.getLocalizedMessage());
        }
    }
}
