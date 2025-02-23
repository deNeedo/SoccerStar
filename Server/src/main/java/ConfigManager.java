import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

public class ConfigManager {
    private static String IP_ADDR = "127.0.0.1";
    private static int PORT = 10001;
    public static String getIP() {
        return IP_ADDR;
    }
    public static int getPORT() {
        return PORT;
    }
    public static void getPreferences() {
        FileReader reader; FileWriter writer; StringBuilder data = new StringBuilder(); int m;
        try {
            reader = new FileReader("./server.config");
            while ((m = reader.read()) != -1) {
                data.append((char) m);
            }
            reader.close();
            String temp = data.toString();
            IP_ADDR = temp.split(":")[0];
            PORT = Integer.parseInt(temp.split(":")[1]);
        } catch (FileNotFoundException e1a) {
            try {
                writer = new FileWriter("./server.config");
                writer.write("127.0.0.1:10001");
                writer.close();
            } catch (IOException e1b) {
                System.out.println("System is unable to access or create the config file in this location.");
            }
        } catch (IOException e2a) {
            System.out.println("System is unable to read the config file in this location.");
        }
    }
}