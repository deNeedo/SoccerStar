import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Random;

public class App {
    private static final int PORT = 54429;
    private static Random random = new Random();
    public static void main(String[] args) {
        try (ServerSocket serverSocket = new ServerSocket(PORT)) {
            System.out.println("Server started");
            while (true) {
                try (Socket clientSocket = serverSocket.accept()) {
                    BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
                    PrintWriter out = new PrintWriter(new OutputStreamWriter(clientSocket.getOutputStream()), true);
                    String message = in.readLine(); String[] data = message.split(" ");
                    if (data[0].equals("LOGIN")) {
                        System.out.println("Client: " + clientSocket.getInetAddress() + " | Login request");
                        File database = new File("./userdata/");
                        String[] dirs = database.list(); boolean flag = true;
                        for (String dir : dirs) {
                            if (dir.equals(data[1])) {
                                File userdata = new File("./userdata/" + data[1] + "/cred");
                                FileReader reader = new FileReader(userdata); int m; String temp = "";
                                while ((m = reader.read()) != -1) {temp += (char) m;}
                                if (data[2].equals(temp)) {out.println("LOGIN 0 " + data[1]); flag = false;}
                                reader.close(); break;
                            }
                        }
                        if (flag == true) {out.println("LOGIN 1");}
                    } else if (data[0].equals("REGISTER")) {
                        System.out.println("Client: " + clientSocket.getInetAddress() + " | Registration request");
                        File database = new File("./userdata/");
                        String[] dirs = database.list(); boolean flag = true;
                        for (String dir : dirs) {
                            if (dir.equals(data[1])) {out.println("REGISTER 1"); flag = false;}
                        }
                        if (flag == true) {
                            File userdata = new File("./userdata/" + data[1]); userdata.mkdirs();
                            userdata = new File("./userdata/" + data[1] + "/cred"); userdata.createNewFile();
                            FileWriter writer = new FileWriter(userdata);
                            writer.write(data[2]); writer.close();
                            userdata = new File("./userdata/" + data[1] + "/stat"); userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("10\t40\t20"); writer.close();
                            userdata = new File("./userdata/" + data[1] + "/item");
                            userdata.createNewFile();

                            userdata = new File("./userdata/" + data[1] + "/star");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("10"); writer.close();
                            userdata = new File("./userdata/" + data[1] + "/endu");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("58"); writer.close(); // default should be 100 // just for test purposes
                            userdata = new File("./userdata/" + data[1] + "/sess");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("10"); writer.close();
                            out.println("REGISTER 0");
                        }
                    }
                    else if (data[0].equals("FETCHSTARS")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Stars fetch request");
                            File userdata = new File("./userdata/" + data[1] + "/star");
                            FileReader reader = new FileReader(userdata);
                            int m;
                            String temp = "";
                            while ((m = reader.read()) != -1) {
                                temp += (char) m;
                            }
                            out.println("FETCHSTARS 0 " + temp);
                            reader.close();
                        } catch (Exception e) {
                            out.println("FETCHSTARS 1");
                        }
                    }
                    else if (data[0].equals("FETCHENDURANCE")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Endurance fetch request");
                            File userdata = new File("./userdata/" + data[1] + "/endu");
                            FileReader reader = new FileReader(userdata);
                            int m;
                            String temp = "";
                            while ((m = reader.read()) != -1) {
                                temp += (char) m;
                            }
                            out.println("FETCHENDURANCE 0 " + temp);
                            reader.close();
                        }
                        catch (Exception e) {
                            out.println("FETCHENDURANCE 1");
                        }
                    }
                    else if (data[0].equals("FETCHSESSIONS")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Sessions fetch request");
                            File userdata = new File("./userdata/" + data[1] + "/sess");
                            FileReader reader = new FileReader(userdata);
                            int m;
                            String temp = "";
                            while ((m = reader.read()) != -1) {
                                temp += (char) m;
                            }
                            out.println("FETCHSESSIONS 0 " + temp);
                            reader.close();
                        }
                        catch (Exception e) {
                            out.println("FETCHSESSIONS 1");
                        }
                    }
                    else if (data[0].equals("FETCHSTATS")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Stats fetch request");
                            File userdata = new File("./userdata/" + data[1] + "/stat");
                            FileReader reader = new FileReader(userdata);
                            int m;
                            String temp = "";
                            while ((m = reader.read()) != -1) {
                                temp += (char) m;
                            }
                            out.println("FETCHSTATS 0 " + temp);
                            reader.close();
                        } catch (Exception e) {
                            out.println("FETCHSTATS 1");
                        }
                    }
                    else if (data[0].equals("FETCHITEMS")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Items fetch request");
                            File userdata = new File("./userdata/" + data[1] + "/item");
                            FileReader reader = new FileReader(userdata); int m; String temp = "";
                            while ((m = reader.read()) != -1) {temp += (char) m;}
                            out.println("FETCHITEMS 0 " + temp);
                            reader.close();
                        }
                        catch (Exception e) {out.println("FETCHITEMS 1");}
                    } else if (data[0].equals("CREATEITEM")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Item create request");
                            String temp = "Item_";
                            for (int m = 0; m < 3; m++) {
                                temp += App.random.nextInt(10);
                            }
                            temp += "\t" + App.random.nextInt(3) + "," + (App.random.nextInt(9) + 1);
                            out.println("CREATEITEM 0 " + temp);
                        } catch (Exception e) {
                            out.println("CREATEITEM 1");
                        }
                    } else if (data[0].equals("UseRelax")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | UseRelax request");
                            String username = data[1];

                            // Fetch current values
                            int currentEndurance = Integer.parseInt(
                                    new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/endu"))).trim());
                            int currentStars = Integer.parseInt(
                                    new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/star"))).trim());
                            int currentSessions = Integer.parseInt(
                                    new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/sess"))).trim());

                            if (currentStars > 0 && currentSessions > 0 && currentEndurance <= 80) {
                                currentEndurance += 20;
                                currentStars -= 1;
                                currentSessions -= 1;

                                try (FileWriter writer = new FileWriter("./userdata/" + username + "/endu")) {
                                    writer.write(String.valueOf(currentEndurance));
                                }
                                try (FileWriter writer = new FileWriter("./userdata/" + username + "/star")) {
                                    writer.write(String.valueOf(currentStars));
                                }
                                try (FileWriter writer = new FileWriter("./userdata/" + username + "/sess")) {
                                    writer.write(String.valueOf(currentSessions));
                                }

                                out.println("UseRelax 0 " + currentEndurance + " " + currentStars + " " + currentSessions);
                            } else {
                                System.out.println("Not enough Stars or Sessions or too much Endurance");
                                out.println("UseRelax 1");
                            }
                        } catch (Exception e) {
                            out.println("UseRelax 1");
                        }
                    }
                    clientSocket.close();
                }
                catch (Exception e) {System.err.println("Client handling error: " + e.getLocalizedMessage());}
            }
        }
        catch (Exception e) {System.err.println("Server error: " + e.getLocalizedMessage());}
    }
}