
import java.util.Calendar;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

public class TaskScheduler {
    private static final ScheduledExecutorService scheduler = Executors.newScheduledThreadPool(1);
    public static void startDailyTask(Runnable task) {
        long initialDelay = TaskScheduler.calculateInitialDelay();
        long period = TimeUnit.DAYS.toMillis(1);
        scheduler.scheduleAtFixedRate(task, initialDelay, period, TimeUnit.MILLISECONDS);
    }
    private static long calculateInitialDelay() {
        Calendar now = Calendar.getInstance();
        Calendar midnight = Calendar.getInstance();
        midnight.set(Calendar.HOUR_OF_DAY, 0);
        midnight.set(Calendar.MINUTE, 0);
        midnight.set(Calendar.SECOND, 0);
        midnight.set(Calendar.MILLISECOND, 0);
        if (now.after(midnight)) {
            midnight.add(Calendar.DAY_OF_MONTH, 1);
        }
        return (midnight.getTimeInMillis() - now.getTimeInMillis());
    }
}