import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;

class ClientHandler implements Runnable {
    final private Socket clientSocket;

    public ClientHandler(Socket clientSocket) {
        this.clientSocket = clientSocket;
    }

    @Override
    public void run() {
        BufferedReader reader; PrintWriter writer;
        try {
            reader = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
            writer = new PrintWriter(new OutputStreamWriter(clientSocket.getOutputStream()), true);
            String message = reader.readLine();
            String[] data = message.split(" ");
            String response = RequestProcessor.processRequest(data);
            // System.out.println(response);
            writer.println(response);
        } catch (IOException e) {
            
        }
    }
}