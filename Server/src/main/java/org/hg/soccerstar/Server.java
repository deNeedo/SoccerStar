package org.hg.soccerstar;
import java.io.*;
import java.net.*;

public class Server {
    private static final int PORT = 54429;

    public static void main(String[] args) {
        try (ServerSocket serverSocket = new ServerSocket(PORT)) {
            new Thread(new NewDayDetector()).start();
            System.out.println("Server started");
            while (true) {
                Socket clientSocket = serverSocket.accept();
                new Thread(new ClientHandler(clientSocket)).start();
            }
        } catch (IOException e) {
            System.err.println("Server error: " + e.getLocalizedMessage());
        }
    }
}
