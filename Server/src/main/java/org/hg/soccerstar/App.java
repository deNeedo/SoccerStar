package org.hg.soccerstar;

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
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.time.temporal.ChronoUnit;
import java.util.Random;
import java.util.ArrayList;
// import java.util.List;
import java.util.Arrays;
import java.util.Collections;
import java.io.IOException;

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
        /* number of traits given clothing will boost */
        int number_of_boosts = (int) Math.floor(1 + Math.random() * 2);
        item += "_" + number_of_boosts;
        /* trait given food will boost and amount of boost in % */
        for (int m = 0; m < number_of_boosts; m++) {
            item += "_" + (int) Math.floor(Math.random() * number_of_traits);
            item += "_" + (int) Math.floor(10 + Math.random() * 10);
        }
        return item;
        /* after rework it will reduce time from training instead of boosting trait */
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
                    String message = in.readLine();
                    String[] data = message.split(" ");
                    if (data[0].equals("LOGIN")) {
                        File database = new File("./userdata/");
                        String[] dirs = database.list();
                        boolean flag = true;
                        for (String dir : dirs) {
                            if (dir.equals(data[1])) {
                                File userdata = new File("./userdata/" + data[1] + "/cred");
                                FileReader reader = new FileReader(userdata);
                                int m;
                                String temp = "";
                                while ((m = reader.read()) != -1) {
                                    temp += (char) m;
                                }
                                if (data[2].equals(temp)) {
                                    out.println("LOGIN 0 " + data[1]);
                                    flag = false;
                                }
                                reader.close();
                                break;
                            }
                        }
                        if (flag == true) {
                            out.println("LOGIN 1");
                        }
                    } else if (data[0].equals("REGISTER")) {
                        File database = new File("./userdata/");
                        String[] dirs = database.list();
                        boolean flag = true;
                        for (String dir : dirs) {
                            if (dir.equals(data[1])) {
                                out.println("REGISTER 1");
                                flag = false;
                            }
                        }
                        if (flag == true) {
                            File userdata = new File("./userdata/" + data[1]);
                            userdata.mkdirs();
                            // hash
                            userdata = new File("./userdata/" + data[1] + "/cred");
                            userdata.createNewFile();
                            FileWriter writer = new FileWriter(userdata);
                            writer.write(data[2]);
                            writer.close();
                            // base traits
                            userdata = new File("./userdata/" + data[1] + "/stat");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            /* <-------- THIS NEEDS TO BE EDITED WHEN WE INTRODUCE ACTUAL TRAITS TO THE GAME -------------> */
                            writer.write("10\t40\t20\t10\t10\t10\t10\t10\t10\t10");
                            /* <------------------------------------------------------------------------------------------> */
                            writer.close();
                            // init equiped items (meaning no equiped items at the start)
                            userdata = new File("./userdata/" + data[1] + "/item");
                            userdata.createNewFile();
                            // init 10 stars
                            userdata = new File("./userdata/" + data[1] + "/star");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("10");
                            writer.close();
                            // init 100 endurance
                            userdata = new File("./userdata/" + data[1] + "/endu");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("80");
                            writer.close();
                            // init 10 available relax sessions
                            userdata = new File("./userdata/" + data[1] + "/sess");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("10");
                            writer.close();
                            // init empty locker
                            userdata = new File("./userdata/" + data[1] + "/lock");
                            userdata.createNewFile();
                            // init shop
                            userdata = new File("./userdata/" + data[1] + "/shop");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            for (int m = 0; m < 4; m++) {
                                if (m == 3) {
                                    writer.write(generateClothing());
                                } else {
                                    writer.write(generateClothing());
                                    writer.write("\n");
                                }
                            }
                            writer.close();
                            userdata = new File("./userdata/" + data[1] + "/cash");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write("7.5");
                            writer.close();
                            // init food item
                            userdata = new File("./userdata/" + data[1] + "/food");
                            userdata.createNewFile();
                            writer = new FileWriter(userdata);
                            writer.write(generateFood());
                            writer.close();
                            out.println("REGISTER 0");
                        }
                    } else if (data[0].equals("FETCHSTARS")) {
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
                    } else if (data[0].equals("FETCHENDURANCE")) {
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
                        } catch (Exception e) {
                            out.println("FETCHENDURANCE 1");
                        }
                    } else if (data[0].equals("FETCHSESSIONS")) {
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
                        } catch (Exception e) {
                            out.println("FETCHSESSIONS 1");
                        }
                    } else if (data[0].equals("FETCHCASH")) {
                        try {
                            File userdata = new File("./userdata/" + data[1] + "/cash");
                            FileReader reader = new FileReader(userdata);
                            int m;
                            String temp = "";
                            while ((m = reader.read()) != -1) {
                                temp += (char) m;
                            }
                            out.println("FETCHCASH 0 " + temp);
                            reader.close();
                        } catch (Exception e) {
                            out.println("FETCHCASH 1");
                        }
                    } else if (data[0].equals("FETCHSTATS")) {
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
                    } else if (data[0].equals("GENERATE_FOOD_ITEM")) {
                        try {

                            Path path = Paths.get("./userdata/", data[1], "/food");
                            List<String> lines = Files.readAllLines(path);
                            String item = generateFood();
                            lines.set(0, item);
                            Files.write(path, lines);
                            out.println("GENERATE_CLOTHING_ITEM 0 " + item);
                        } catch (Exception e) {
                            out.println("GENERATE_CLOTHING_ITEM 1");
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
                    } else if (data[0].equals("UseRelax")) {
                        try {
                            String username = data[1];

                            // Fetch current values
                            int currentEndurance = Integer.parseInt(
                                    new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/endu")))
                                            .trim());
                            int currentStars = Integer.parseInt(
                                    new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/star")))
                                            .trim());
                            int currentSessions = Integer.parseInt(
                                    new String(Files.readAllBytes(Paths.get("./userdata/" + username + "/sess")))
                                            .trim());

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

                                out.println(
                                        "UseRelax 0 " + currentEndurance + " " + currentStars + " " + currentSessions);
                            } else {
                                out.println("UseRelax 1");
                            }
                        } catch (Exception e) {
                            out.println("UseRelax 1");
                        }
                    } else if (data[0].equals("STARTWORK")) {
                        try {
                            String username = data[1];
                            int hours = Integer.parseInt(data[2]);

                            File trainingFile = new File("./userdata/" + username + "/curr_trai");
                            if (trainingFile.exists()) {
                                out.println("STARTWORK 1 PlayerIsTraining");
                                continue;
                            }

                            if (hours > 10 || hours < 1) {
                                throw new Exception("too many or too low hours");
                            }

                            LocalDateTime startTime = LocalDateTime.now();
                            LocalDateTime endTime = startTime.plusHours(hours);

                            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

                            String startTimeFormatted = startTime.format(formatter);
                            String endTimeFormatted = endTime.format(formatter);

                            File workFile = new File("./userdata/" + username + "/work");
                            FileWriter writer = new FileWriter(workFile);
                            writer.write(startTimeFormatted + "\n" + endTimeFormatted);
                            writer.close();

                            out.println("STARTWORK 0");
                        } catch (Exception e) {
                            out.println("STARTWORK 1");
                        }
                    } else if (data[0].equals("CANCELWORK")) {
                        try {
                            String username = data[1];
                            File workFile = new File("./userdata/" + username + "/work");
                            if (workFile.exists()) {
                                workFile.delete();
                            }

                            out.println("CANCELWORK 0");
                        } catch (Exception e) {
                            out.println("CANCELWORK 1");
                        }
                    } else if (data[0].equals("CHECKWORK")) {
                        try {
                            String username = data[1];

                            File workFile = new File("./userdata/" + username + "/work");

                            if (workFile.exists()) {
                                BufferedReader reader = new BufferedReader(new FileReader(workFile));
                                String startTimeStr = reader.readLine();
                                String endTimeStr = reader.readLine();
                                reader.close();

                                DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
                                LocalDateTime endTime = LocalDateTime.parse(endTimeStr, formatter);
                                LocalDateTime currentTime = LocalDateTime.now();

                                if (currentTime.isAfter(endTime)) {

                                    // Work completed, 
                                    // TODO add cash pattern
                                    double cashToAdd = ChronoUnit.HOURS
                                            .between(LocalDateTime.parse(startTimeStr, formatter), endTime) * 5;

                                    File cashFile = new File("./userdata/" + username + "/cash");
                                    double currentCash = Double
                                            .parseDouble(new String(Files.readAllBytes(cashFile.toPath())).trim());
                                    currentCash += cashToAdd;

                                    try (FileWriter writer = new FileWriter(cashFile)) {
                                        writer.write(String.valueOf(currentCash));
                                    }

                                    workFile.delete();

                                    out.println("CHECKWORK 0 " + currentCash);
                                } else {
                                    out.println("CHECKWORK 1 " + startTimeStr + " " + endTimeStr);
                                }
                            } else {
                                out.println("CHECKWORK 1");
                            }
                        } catch (Exception e) {
                            out.println("CHECKWORK 1");
                        }
                    }
                    else if (data[0].equals("STARTTRAINING")) {
                        try {
                             System.out.println("Client: " + clientSocket.getInetAddress() + " | Start Training request");
                            String username = data[1];
                            int trainingNumber = Integer.parseInt(data[2]);

                            File workFile = new File("./userdata/" + username + "/work");
                            if (workFile.exists()) {
                                out.println("STARTTRAINING 1 PlayerIsWorking");
                                continue;
                            }

                            if (new File("./userdata/" + username + "/curr_trai").exists())
                            {
                                out.println("STARTTRAINING 1 PlayerIsTraining");
                                continue;
                            }

                            BufferedReader currentTrainingReader = new BufferedReader(new FileReader(new File("./userdata/" + username + "/trai")));
                            String[] trainings = new String[3];
                            for (int i = 0; i < 3; i++) {
                                trainings[i] = currentTrainingReader.readLine();
                            }
                            currentTrainingReader.close();

                            BufferedReader trainingsReader = new BufferedReader(new FileReader(new File("./trainings/" + trainings[trainingNumber])));
                            trainingsReader.readLine();
                            trainingsReader.readLine();
                            String traitAddStr = trainingsReader.readLine();
                            String durationStr = trainingsReader.readLine();
                            trainingsReader.close();
                            
                            int traitAdd = Integer.parseInt(traitAddStr);
                            int enduranceCost = traitAdd * 10;

                            File enduranceFile = new File("./userdata/" + username + "/endu");
                            BufferedReader enduranceReader = new BufferedReader(new FileReader(enduranceFile));
                            int endurance = Integer.parseInt(enduranceReader.readLine());
                            enduranceReader.close();

                            if (endurance < enduranceCost) {
                                out.println("STARTTRAINING 1 NotEnoughEndurance");
                                continue;
                            }

                            endurance -= enduranceCost;
                            FileWriter enduranceWriter = new FileWriter(enduranceFile);
                            enduranceWriter.write(String.valueOf(endurance));
                            enduranceWriter.close();

                            LocalDateTime startTime = LocalDateTime.now();
                            LocalDateTime endTime = startTime.plusMinutes(Integer.parseInt(durationStr));
                            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
                            String endTimeFormatted = endTime.format(formatter);

                            FileWriter writer = new FileWriter(new File("./userdata/" + username + "/curr_trai"));

                            writer.write(trainings[trainingNumber] + "\n" + durationStr + "\n" + endTimeFormatted);
                            writer.close();

                            out.println("STARTTRAINING 0");
                        } catch (Exception e) {
                            out.println("STARTTRAINING 1");
                            e.printStackTrace();
                        }
                    }
                    else if (data[0].equals("STOPTRAINING")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Stop Training request");
                            String username = data[1];
                            File trainingFile = new File("./userdata/" + username + "/curr_trai");

                            if (trainingFile.exists()) {
                                BufferedReader trainingReader = new BufferedReader(new FileReader(trainingFile));
                                String trainingTitle = trainingReader.readLine();
                                trainingReader.close();

                                BufferedReader trainingsReader = new BufferedReader(new FileReader(new File("./trainings/" + trainingTitle)));
                                trainingsReader.readLine();
                                trainingsReader.readLine();
                                String traitAddStr = trainingsReader.readLine();
                                trainingsReader.close();
                                
                                int traitAdd = Integer.parseInt(traitAddStr);
                                int enduranceRestore = traitAdd * 10;

                                File enduranceFile = new File("./userdata/" + username + "/endu");
                                BufferedReader enduranceReader = new BufferedReader(new FileReader(enduranceFile));
                                int endurance = Integer.parseInt(enduranceReader.readLine());
                                enduranceReader.close();

                                endurance += enduranceRestore;
                                FileWriter enduranceWriter = new FileWriter(enduranceFile);
                                enduranceWriter.write(String.valueOf(endurance));
                                enduranceWriter.close();

                                trainingFile.delete();
                                out.println("STOPTRAINING 0");
                            } else {
                                out.println("STOPTRAINING 1 NotTraining");
                            }
                        } catch (Exception e) {
                            out.println("STOPTRAINING 1");
                        }
                    }
                    else if (data[0].equals("FETCHTRAINING")) {
                        try {
                            System.out.println("Client: " + clientSocket.getInetAddress() + " | Fetch Training request");
                            String username = data[1];
                            File currentTraining = new File("./userdata/" + username + "/curr_trai");
                            File availableTrainings = new File("./userdata/" + username + "/trai");
                            
                            if (currentTraining.exists()) {
                                BufferedReader reader = new BufferedReader(new FileReader(currentTraining));
                                String trainingFileStr = reader.readLine();
                                String durationStr = reader.readLine();
                                String endTimeStr = reader.readLine();
                                reader.close();

                                reader = new BufferedReader(new FileReader(new File("./trainings/" + trainingFileStr)));
                                String trainingTitle = reader.readLine().replaceAll(" ", "|");
                                String trainingDescription = reader.readLine().replaceAll(" ", "|");
                                String traitStr = reader.readLine();
                                int trait = Integer.parseInt(traitStr);
                                reader.close();

                                DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
                                LocalDateTime endTime = LocalDateTime.parse(endTimeStr, formatter);

                                if (!LocalDateTime.now().isAfter(endTime)) {
                                    out.println("FETCHTRAINING 0 " + trainingTitle + " " + trainingDescription + " "
                                            + durationStr + " " + endTimeStr.replaceAll(" ", "|"));
                                    continue;
                                }
                                
                                File statsFile = new File("./userdata/" + username + "/stat");
                                BufferedReader statReader = new BufferedReader(new FileReader(statsFile));
                                String stats = statReader.readLine();
                                statReader.close();

                                String[] statStrings = stats.split("\t");
                                int[] statInts = new int[statStrings.length];
                                for (int i = 0; i < statStrings.length; i++) {
                                    statInts[i] = Integer.parseInt(statStrings[i]);
                                }
                                switch (trainingFileStr) {
                                    case "dribbling":
                                        statInts[0] += trait;
                                        break;
                                    case "fitness":
                                        statInts[1] += trait;
                                        break;
                                    case "kicking":
                                        statInts[2] += trait;
                                        break;
                                    case "marking":
                                        statInts[3] += trait;
                                        break;
                                    case "passing":
                                        statInts[4] += trait;
                                        break;
                                    case "reflex":
                                        statInts[5] += trait;
                                        break;
                                    case "shooting":
                                        statInts[6] += trait;
                                        break;
                                    case "speed":
                                        statInts[7] += trait;
                                        break;
                                    case "tackling":
                                        statInts[8] += trait;
                                        break;
                                    case "throwing":
                                        statInts[9] += trait;
                                        break;
                                    default:
                                        System.out.println("Unknown training type: " + trainingFileStr);
                                        break;
                                }

                                StringBuilder updatedStats = new StringBuilder();
                                for (int i = 0; i < statInts.length; i++) {
                                    updatedStats.append(statInts[i]);
                                    if (i < statInts.length - 1) {
                                        updatedStats.append("\t");
                                    }
                                }

                                FileWriter statWriter = new FileWriter(statsFile);
                                statWriter.write(updatedStats.toString());
                                statWriter.close();

                                currentTraining.delete();
                                availableTrainings.delete();

                                String[] trainingFiles = { "dribbling", "fitness", "kicking", "marking", "passing",
                                "reflex", "shooting", "speed", "tackling", "throwing" };
                                List<String> trainingList = Arrays.asList(trainingFiles);
                                Collections.shuffle(trainingList);

                                FileWriter fileWriter = new FileWriter(availableTrainings);
                                fileWriter.write(
                                        trainingList.get(0) + "\n" + trainingList.get(1) + "\n" + trainingList.get(2));
                                fileWriter.close();

                                out.println("FETCHTRAINING 0 " + fetchTrainingData(trainingList.subList(0, 3)));
                            } else if (availableTrainings.exists()) {
                                BufferedReader reader_trai = new BufferedReader(new FileReader(availableTrainings));
                                List<String> trainingFiles = new ArrayList<>();
                                trainingFiles.add(reader_trai.readLine());
                                trainingFiles.add(reader_trai.readLine());
                                trainingFiles.add(reader_trai.readLine());
                                reader_trai.close();

                                out.println("FETCHTRAINING 0 " + fetchTrainingData(trainingFiles));
                            } else {
                                String[] trainingFiles = { "dribbling", "fitness", "kicking", "marking", "passing",
                                "reflex", "shooting", "speed", "tackling", "throwing" };
                                List<String> trainingList = Arrays.asList(trainingFiles);
                                Collections.shuffle(trainingList);

                                FileWriter fileWriter = new FileWriter(availableTrainings);
                                fileWriter.write(
                                        trainingList.get(0) + "\n" + trainingList.get(1) + "\n" + trainingList.get(2));
                                fileWriter.close();

                                out.println("FETCHTRAINING 0 " + fetchTrainingData(trainingList.subList(0, 3)));
                            }
                        } catch (Exception e) {
                            out.println("FETCHTRAINING 1 :");
                            e.printStackTrace();
                        }
                    }
                    clientSocket.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        } catch (Exception e) {
            System.err.println("Server error: " + e.getLocalizedMessage());
        }
    }
    
    static StringBuilder fetchTrainingData(List<String> trainingFiles) throws IOException {
        StringBuilder trainings = new StringBuilder();
        for (String file : trainingFiles) {
            BufferedReader reader = new BufferedReader(new FileReader("./trainings/" + file));
            trainings.append(reader.readLine().replaceAll(" ", "|")).append(" ")
                     .append(reader.readLine().replaceAll(" ", "|")).append(" ")
                     .append(reader.readLine()).append(" ")
                     .append(reader.readLine()).append(" ");
            reader.close();
        }
        return trainings;
    }
}