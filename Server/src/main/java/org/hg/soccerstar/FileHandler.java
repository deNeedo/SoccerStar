package org.hg.soccerstar;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.List;
import java.io.InputStream;
import java.io.InputStreamReader;

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



}
