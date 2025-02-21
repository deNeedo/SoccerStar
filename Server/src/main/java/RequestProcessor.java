import java.io.IOException;

public class RequestProcessor {
    public static String processRequest(String[] data) throws IOException {
        String command = data[0];
        return switch (command) {
            case "LOGIN" -> RequestCommands.login(data);
            case "REGISTER" -> RequestCommands.register(data);
            case "FETCHSTARS" -> RequestCommands.fetchStars(data);
            case "FETCHENDURANCE" -> RequestCommands.fetchEndurance(data);
            case "FETCHSESSIONS" -> RequestCommands.fetchSessions(data);
            case "FETCHCASH" -> RequestCommands.fetchCash(data);
            case "FETCHSTATS" -> RequestCommands.fetchStats(data);
            case "GENERATE_FOOD_ITEM" -> RequestCommands.generateFoodItem(data);
            case "GENERATE_CLOTHING_ITEM" -> RequestCommands.generateClothingItem(data);
            case "FETCH_FOOD_ITEM" -> RequestCommands.fetchFood(data);
            case "FETCH_CLOTHING_ITEMS" -> RequestCommands.fetchClothing(data);
            case "FETCHLOCKERITEMS" -> RequestCommands.fetchLockerItems(data);
            case "BUY_CLOTHING_ITEM" -> RequestCommands.buyClothingItem(data);
            case "USERELAX" -> RequestCommands.useRelax(data);
            case "STARTWORK" -> RequestCommands.startWork(data);
            case "CANCELWORK" -> RequestCommands.cancelWork(data);
            case "CHECKWORK" -> RequestCommands.checkWork(data);
            case "STARTTRAINING" -> RequestCommands.startTraining(data);
            case "STOPTRAINING" -> RequestCommands.stopTraining(data);
            case "FETCHTRAINING" -> RequestCommands.fetchTraining(data);
            default -> "ERROR UnknownCommand";
        };
    }
}
