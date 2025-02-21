import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

public class DailyReset extends Thread {
    @Override
    public void run() {
        File database = new File("./userdata/");
        String[] dirs = database.list();
        for (String dir : dirs) {
            try {
                File userdata = new File("./userdata/" + dir + "/endu");
                FileWriter writer = new FileWriter(userdata);
                writer.write("100"); writer.close();
                userdata = new File("./userdata/" + dir + "/sess");
                writer = new FileWriter(userdata);
                writer.write("10"); writer.close();
            } catch (IOException e) {
                System.out.println(e.getLocalizedMessage());
            }
        }
    }
}