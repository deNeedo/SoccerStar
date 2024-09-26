package org.hg.soccerstar;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

public class FileHandler {
    private static String fetchFileContent(String username, String fileName) throws FileNotFoundException, IOException {
        String content = "";
        File file = new File("./userdata/" + username + "/" + fileName);
        try (FileReader reader = new FileReader(file)) {
            int m;
            StringBuilder temp = new StringBuilder();
            while ((m = reader.read()) != -1) {
                temp.append((char) m);
            }
            content = temp.toString();
        }
        return content;
    }

    private static void writeFileContent(String username, String fileName, String content) throws IOException {
        Path path = Paths.get("./userdata/", username, fileName);
        Files.write(path, List.of(content));
    }

    public static String fetchCred(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "cred");
    }
    public static String fetchStars(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "star");
    }
    public static String fetchEndurance(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "endu");
    }
    public static String fetchSessions(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "sess");
    }
    public static String fetchClothing(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "shop");
    }
    public static String fetchFood(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "food");
    }
    public static String fetchCash(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "cash");
    }
    public static String fetchStats(String username) throws FileNotFoundException, IOException {
        return fetchFileContent(username, "stat");
    }

    public static void editStars(String username, double newStars) throws IOException {
        writeFileContent(username, "star", String.format("%.2f", newStars));
    }

    public static void editCash(String username, double newCash) throws IOException {
        writeFileContent(username, "cash", String.format("%.2f", newCash));
    }

    public static void editEndurance(String username, int newEndurance) throws IOException {
        writeFileContent(username, "endu", Integer.toString(newEndurance));
    }

    public static void editSessions(String username, int newSessions) throws IOException {
        writeFileContent(username, "sess", Integer.toString(newSessions));
    }

    public static void editClothing(String username, List<String> shopItems) throws IOException {
        Path path = Paths.get("./userdata/", username, "shop");
        Files.write(path, shopItems);
    }

    public static void editLocker(String username, List<String> lockerItems) throws IOException {
        Path path = Paths.get("./userdata/", username, "lock");
        Files.write(path, lockerItems);
    }

    
    public static StringBuilder fetchTrainingData(List<String> trainingFiles) throws IOException {

        StringBuilder trainings = new StringBuilder();
        ClassLoader classLoader = FileHandler.class.getClassLoader();

        for (String file : trainingFiles) {
            try (InputStream inputStream = classLoader.getResourceAsStream("trainings/" + file);
                 BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream))) {
                 
                trainings.append(reader.readLine().replaceAll(" ", "|")).append(" ")
                        .append(reader.readLine().replaceAll(" ", "|")).append(" ")
                        .append(reader.readLine()).append(" ")
                        .append(reader.readLine()).append(" ");
            }
        }
        return trainings;
    }

    public static List<String> fetchShopItems(String username) throws FileNotFoundException, IOException {
        File shopFile = new File("./userdata/" + username + "/shop");
        List<String> shopItems = new ArrayList<>();

        try (BufferedReader reader = new BufferedReader(new FileReader(shopFile))) {
            String line;
            while ((line = reader.readLine()) != null) {
                shopItems.add(line);
            }
        }
        return shopItems;
    }
    
    public static List<String> fetchLockerItems(String username) throws FileNotFoundException, IOException {
        File lockerFile = new File("./userdata/" + username + "/lock");

        List<String> lockerItems = Files.readAllLines(lockerFile.toPath());
        while (lockerItems.size() < 4) { 
            lockerItems.add("");
        }

        return lockerItems;
    }

}
