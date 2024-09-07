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
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;
import java.util.Random;
 
public class App {
    private static final int PORT = 54429;
    private static final int number_of_traits = 3;
    private static Random random = new Random();
    private static String generateClothing() {
        /* name of the item with 3 digit designator */
        String item = "clothing_";
        for (int m = 0; m < 3; m++) {
            item += App.random.nextInt(10);
        }
        /* number of traits given clothing will boost */
        int number_of_boosts = (int) Math.floor(1 + Math.random() * 5);
        item += "_" + number_of_boosts;
        /* trait given clothing will boost and amount of boost (as a integer value) */
        for (int m = 0; m < number_of_boosts; m++) {
            item += "_" + (int) Math.floor(Math.random() * number_of_traits);
            item += "_" + (int) Math.floor(1 + Math.random() * 10);
        }
        return item;
    }
    private static String generateFood() {
        /* name of the item with 3 digit designator */
        String item = "food_";
        for (int m = 0; m < 3; m++) {
            item += App.random.nextInt(10);
        }
        /* trait given food will boost and amount of boost in % */
        /* ------------------------------------------------------------------------ */
        item += "_" + (int) Math.floor(Math.random() * number_of_traits);
        /* after rework it will reduce time from training instead of boosting trait */
        item += "_" + (int) Math.floor(10 + Math.random() * 10);
        return item;
    }
    private static String fetchClothing(String username) {
        String items = "";
        try {
            items = new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/shop")));
        } catch (Exception e) {
            items = "";
        }
        return items;
    }
    private static String fetchFood(String username) {
        String items = "";
        try {
            items = new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/food")));
        } catch (Exception e) {
            items = "";
        }
        return items;
    }
    public static void main(String[] args) {
        try (ServerSocket serverSocket = new ServerSocket(PORT)) {
            new Thread(new NewDayDetector()).start();
            System.out.println("Server started");
            while (true) {
                try (Socket clientSocket = serverSocket.accept()) {
                    BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
                    PrintWriter out = new PrintWriter(new OutputStreamWriter(clientSocket.getOutputStream()), true);
                    String message = in.readLine(); String[] data = message.split(" ");
                    if (data[0].equals("LOGIN")) {
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
                        File database = new File("./userdata/");
                        String[] dirs = database.list(); boolean flag = true;
                        for (String dir : dirs) {
                            if (dir.equals(data[1])) {out.println("REGISTER 1"); flag = false;}
                        }
                        if (flag == true) {
                            File userdata = new File("./userdata/" + data[1]); userdata.mkdirs();
                            // hash
                            userdata = new File("./userdata/" + data[1] + "/cred"); userdata.createNewFile();
                            FileWriter writer = new FileWriter(userdata); writer.write(data[2]); writer.close();
                            // base traits
                            userdata = new File("./userdata/" + data[1] + "/stat"); userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            /* <-------- THIS NEEDS TO BE EDITED WHEN WE INTRODUCE ACTUAL TRAITS TO THE GAME -------------> */
                            writer.write("10\t40\t20");
                            /* <------------------------------------------------------------------------------------------> */
                            writer.close();
                            // init equiped items (meaning no equiped items at the start)
                            userdata = new File("./userdata/" + data[1] + "/item"); userdata.createNewFile();
                            // init 10 stars
                            userdata = new File("./userdata/" + data[1] + "/star"); userdata.createNewFile();
                            writer = new FileWriter(userdata); writer.write("10"); writer.close();
                            // init 100 endurance
                            userdata = new File("./userdata/" + data[1] + "/endu"); userdata.createNewFile();
                            writer = new FileWriter(userdata); writer.write("100"); writer.close();
                            // init 10 available relax sessions
                            userdata = new File("./userdata/" + data[1] + "/sess"); userdata.createNewFile();
                            writer = new FileWriter(userdata); writer.write("10"); writer.close();
                            // init empty locker
                            userdata = new File("./userdata/" + data[1] + "/lock"); userdata.createNewFile();
                            // init shop
                            userdata = new File("./userdata/" + data[1] + "/shop"); userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            for (int m = 0; m < 4; m++) {
                                if (m == 3) {writer.write(generateClothing());}
                                else {writer.write(generateClothing()); writer.write("\n");}
                            }
                            writer.close();
                            // init food item
                            userdata = new File("./userdata/" + data[1] + "/food"); userdata.createNewFile();
                            writer = new FileWriter(userdata); writer.write(generateFood()); writer.close();
                            out.println("REGISTER 0");
                        }
                    }
                    else if (data[0].equals("FETCHSTARS")) {
                        try {
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
                    else if (data[0].equals("GENERATE_FOOD_ITEM")) {
                        try {
                            out.println("GENERATE_FOOD_ITEM 0 " + generateFood());
                        } catch (Exception e) {
                            out.println("GENERATE_FOOD_ITEM 1");
                        }
                    } else if (data[0].equals("GENERATE_CLOTHING_ITEM")) {
                        try {
                            Path path = Paths.get("./userdata/", data[1], "/shop");
                            List<String> lines = Files.readAllLines(path);
                            String item = generateClothing();
                            lines.set(Integer.parseInt(data[2]), item);
                            Files.write(path, lines);
                            out.println("GENERATE_CLOTHING_ITEM 0 " + item);
                        } catch (Exception e) {
                            out.println("GENERATE_CLOTHING_ITEM 1");
                        }
                    } else if (data[0].equals("FETCH_FOOD_ITEM")) {
                        try {
                            out.println("FETCH_FOOD_ITEM 0 " + fetchFood(data[1]));
                        } catch (Exception e) {
                            out.println("FETCH_FOOD_ITEM 1");
                        }
                    } else if (data[0].equals("FETCH_CLOTHING_ITEMS")) {
                        try {
                            out.println("FETCH_CLOTHING_ITEMS 0 " + fetchClothing(data[1]));
                        } catch (Exception e) {
                            out.println("FETCH_CLOTHING_ITEMS 1");
                        }
                    } 
                    else if (data[0].equals("UseRelax")) {
                        try {
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