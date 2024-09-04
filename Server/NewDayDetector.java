import java.util.Calendar;
import java.io.File;
import java.io.FileWriter;

public class NewDayDetector extends Thread {
    @Override
    public void run() {
        while (true) {
            long time_to_sleep = this.calculateTime();
            try {Thread.sleep(time_to_sleep);}
            catch (Exception e) {e.printStackTrace();}
            this.newDayReset();
        }

    }
    private void newDayReset() {
        File database = new File("./userdata/");
        String[] dirs = database.list();
        for (String dir : dirs) {
            try {
                File userdata = new File("./userdata/" + dir + "/endu");
                FileWriter writer = new FileWriter(userdata);
                writer.write("100"); writer.close();
            } catch (Exception e) {/* put here some nice error about missing / unaccesible files */}
        }
    }
    private long calculateTime() {
        Calendar now = Calendar.getInstance();
        Calendar midnight = Calendar.getInstance();
        midnight.set(Calendar.HOUR_OF_DAY, 0);
        midnight.set(Calendar.MINUTE, 0);
        midnight.set(Calendar.SECOND, 0);
        midnight.set(Calendar.MILLISECOND, 0);
        if (now.after(midnight)) {
            midnight.add(Calendar.DAY_OF_MONTH, 1);
        }
        return midnight.getTimeInMillis() - now.getTimeInMillis();
    }
}
