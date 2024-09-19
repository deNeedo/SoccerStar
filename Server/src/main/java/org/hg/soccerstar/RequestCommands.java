package org.hg.soccerstar;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.Random;

public class RequestCommands {
    private static Random random = new Random();
    private static final int number_of_traits = 3;

    // Helpers
    private static String generateClothing() {
        /* name of the item with 3 digit designator */
        String item = "clothing_";
        for (int m = 0; m < 3; m++) {
            item += random.nextInt(10);
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
            item += random.nextInt(10);
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
    
    // Main Functions
    public static String login(String[] data) throws IOException {
        File database = new File("./userdata/");
        String[] dirs = database.list();
        for (String dir : dirs) {
            if (dir.equals(data[1])) {
                String temp = FileHandler.fetchCred(data[1]);
                if (data[2].equals(temp)) {
                    return "LOGIN 0 " + data[1];
                }
                break;
            }
        }
        return "LOGIN 1";
    }
    public static String register(String[] data) throws IOException {
        File database = new File("./userdata/");
        String[] dirs = database.list();
        boolean flag = true;
        for (String dir : dirs) {
            if (dir.equals(data[1])) {
                return "REGISTER 1";
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
            writer.write("100");
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
            return "REGISTER 0";
        }
        return "REGISTER 1";
    }
    public static String fetchStars(String[] data)  {
        try {
            return "FETCHSTARS 0 " + FileHandler.fetchStars(data[1]);
        } catch (Exception e) {
            return "FETCHSTARS 1";
        }
    }
    public static String fetchEndurance(String[] data) throws IOException {
        try {
            return "FETCHENDURANCE 0 " + FileHandler.fetchEndurance(data[1]);
        } catch (Exception e) {
            return "FETCHENDURANCE 1";
        }
    }
    public static String fetchSessions(String[] data) throws IOException {
        try {
            return "FETCHSESSIONS 0 " + FileHandler.fetchSessions(data[1]);
        } catch (Exception e) {
            return "FETCHSESSIONS 1";
        }
    }
    public static String fetchCash(String[] data) throws IOException {
        try {
            return "FETCHCASH 0 " + FileHandler.fetchCash(data[1]);
        } catch (Exception e) {
            return "FETCHCASH 1";
        }
    }
    public static String fetchStats(String[] data) throws IOException {
        try {
            return "FETCHSTATS 0 " + FileHandler.fetchStats(data[1]);
        } catch (Exception e) {
            return "FETCHSTATS 1";
        }
    }
    public static String generateFoodItem(String[] data) throws IOException {
        try {
            Path path = Paths.get("./userdata/", data[1], "/food");
            List<String> lines = Files.readAllLines(path);
            String item = generateFood();
            lines.set(0, item);
            Files.write(path, lines);
            return "GENERATE_CLOTHING_ITEM 0 " + item;
        } catch (Exception e) {
            return "GENERATE_CLOTHING_ITEM 1";
        }
    }
    public static String generateClothingItem(String[] data) throws IOException {
        try {
            Path path = Paths.get("./userdata/", data[1], "/shop");
            List<String> lines = Files.readAllLines(path);
            String item = generateClothing();
            lines.set(Integer.parseInt(data[2]), item);
            Files.write(path, lines);
            return "GENERATE_CLOTHING_ITEM 0 " + item;
        } catch (Exception e) {
            return "GENERATE_CLOTHING_ITEM 1";
        }
    }
    public static String fetchFood(String[] data) {
        try {
            return "FETCHFOOD 0 " + FileHandler.fetchFood(data[1]);
        } catch (Exception e) {
            return "FETCHFOOD 1";
        }
    }
    public static String fetchClothing(String[] data) throws IOException {
        try {
            return "FETCHCLOTHING 0 " + FileHandler.fetchClothing(data[1]);
        } catch (Exception e) {
            return "FETCHCLOTHING 1";
        }
    }
    public static String useRelax(String[] data) throws IOException {
        try {
            String username = data[1];
            
            int currentEndurance = Integer.parseInt(FileHandler.fetchEndurance(username));
            int currentStars = Integer.parseInt(FileHandler.fetchStars(username));
            int currentSessions = Integer.parseInt(FileHandler.fetchSessions(username));

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

                return ("USERELAX 0 " + currentEndurance + " " + currentStars + " " + currentSessions);
            } else {
                return "USERELAX 1";
            }
        } catch (Exception e) {
            return "USERELAX 1";
        }
    }

    public static String startWork(String[] data) throws IOException {
        try {
            String username = data[1];
            int hours = Integer.parseInt(data[2]);

            File trainingFile = new File("./userdata/" + username + "/curr_trai");
            if (trainingFile.exists()) {
                return "STARTWORK 1 PlayerIsTraining";
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

            return "STARTWORK 0";
        } catch (Exception e) {
            return "STARTWORK 1";
        }
    }

    public static String cancelWork(String[] data) throws IOException {
        try {
            String username = data[1];
            File workFile = new File("./userdata/" + username + "/work");
            if (workFile.exists()) {
                workFile.delete();
            }

            return ("CANCELWORK 0");
        } catch (Exception e) {
            return ("CANCELWORK 1");
        }

    }

    public static String checkWork(String[] data) throws IOException {
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
                    //TODO add cash pattern
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

                    return "CHECKWORK 0 " + currentCash;
                } else {
                    return "CHECKWORK 1 " + startTimeStr + " " + endTimeStr;
                }
            } else {
                return "CHECKWORK 1";
            }
        } catch (Exception e) {
            return "CHECKWORK 1";
        }
    }
    
    public static String startTraining(String[] data) throws IOException {
        try {
            String username = data[1];
            int trainingNumber = Integer.parseInt(data[2]);

            File workFile = new File("./userdata/" + username + "/work");
            if (workFile.exists()) {
                return "STARTTRAINING 1 PlayerIsWorking";
            }

            if (new File("./userdata/" + username + "/curr_trai").exists()) {
                return "STARTTRAINING 1 PlayerIsTraining";
            }

            BufferedReader currentTrainingReader = new BufferedReader(
                    new FileReader(new File("./userdata/" + username + "/trai")));
            String[] trainings = new String[3];
            for (int i = 0; i < 3; i++) {
                trainings[i] = currentTrainingReader.readLine();
            }
            currentTrainingReader.close();

            ClassLoader classLoader = RequestCommands.class.getClassLoader();
            InputStream trainingFileStream = classLoader.getResourceAsStream("trainings/" + trainings[trainingNumber]);
            BufferedReader trainingsReader = new BufferedReader(new InputStreamReader(trainingFileStream));
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
                return "STARTTRAINING 1 NotEnoughEndurance";
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

            return "STARTTRAINING 0";
        } catch (Exception e) {
            return "STARTTRAINING 1";
        }
    }
    public static String stopTraining(String[] data) throws IOException {
        try {
            String username = data[1];
            File trainingFile = new File("./userdata/" + username + "/curr_trai");

            if (trainingFile.exists()) {
                BufferedReader trainingReader = new BufferedReader(new FileReader(trainingFile));
                String trainingTitle = trainingReader.readLine();
                trainingReader.close();

                ClassLoader classLoader = RequestCommands.class.getClassLoader();
                InputStream trainingFileStream = classLoader.getResourceAsStream("trainings/" + trainingTitle);
                BufferedReader trainingsReader = new BufferedReader(new InputStreamReader(trainingFileStream));
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
                return "STOPTRAINING 0";
            } else {
                return "STOPTRAINING 1 NotTraining";
            }
        } catch (Exception e) {
            return "STOPTRAINING 1";
        }
    }
    public static String fetchTraining(String[] data) throws IOException {
        try {
            String username = data[1];
            File currentTraining = new File("./userdata/" + username + "/curr_trai");
            File availableTrainings = new File("./userdata/" + username + "/trai");
            
            if (currentTraining.exists()) {
                BufferedReader reader = new BufferedReader(new FileReader(currentTraining));
                String trainingFileStr = reader.readLine();
                String durationStr = reader.readLine();
                String endTimeStr = reader.readLine();
                reader.close();

                ClassLoader classLoader = RequestCommands.class.getClassLoader();
                InputStream trainingFileStream = classLoader.getResourceAsStream("trainings/" + trainingFileStr);
                BufferedReader trainingReader = new BufferedReader(new InputStreamReader(trainingFileStream));
                String trainingTitle = trainingReader.readLine().replaceAll(" ", "|");
                String trainingDescription = trainingReader.readLine().replaceAll(" ", "|");
                String traitStr = trainingReader.readLine();
                int trait = Integer.parseInt(traitStr);
                trainingReader.close();

                DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
                LocalDateTime endTime = LocalDateTime.parse(endTimeStr, formatter);

                if (!LocalDateTime.now().isAfter(endTime)) {
                    return ("FETCHTRAINING 0 " + trainingTitle + " " + trainingDescription + " "
                            + durationStr + " " + endTimeStr.replaceAll(" ", "|"));
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

                return "FETCHTRAINING 0 " + FileHandler.fetchTrainingData(trainingList.subList(0, 3));
            } else if (availableTrainings.exists()) {
                BufferedReader reader_trai = new BufferedReader(new FileReader(availableTrainings));
                List<String> trainingFiles = new ArrayList<>();
                trainingFiles.add(reader_trai.readLine());
                trainingFiles.add(reader_trai.readLine());
                trainingFiles.add(reader_trai.readLine());
                reader_trai.close();

                return "FETCHTRAINING 0 " + FileHandler.fetchTrainingData(trainingFiles);
            } else {
                String[] trainingFiles = { "dribbling", "fitness", "kicking", "marking", "passing",
                "reflex", "shooting", "speed", "tackling", "throwing" };
                List<String> trainingList = Arrays.asList(trainingFiles);
                Collections.shuffle(trainingList);

                FileWriter fileWriter = new FileWriter(availableTrainings);
                fileWriter.write(
                        trainingList.get(0) + "\n" + trainingList.get(1) + "\n" + trainingList.get(2));
                fileWriter.close();

                return "FETCHTRAINING 0 " + FileHandler.fetchTrainingData(trainingList.subList(0, 3));
            }
        } catch (Exception e) {
            return "FETCHTRAINING 1 :";
        }
    }

}