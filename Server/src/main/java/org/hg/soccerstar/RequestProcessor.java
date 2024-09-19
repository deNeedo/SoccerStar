package org.hg.soccerstar;
import java.io.*;

public class RequestProcessor {

    public static String processRequest(String[] data) throws IOException {
        String command = data[0];
        switch (command) {
            case "LOGIN":
                return RequestCommands.login(data);
            case "REGISTER":
                return RequestCommands.register(data);
            case "FETCHSTARS":
                return RequestCommands.fetchStars(data);
            case "FETCHENDURANCE":
                return RequestCommands.fetchEndurance(data);
            case "FETCHSESSIONS":
                return RequestCommands.fetchSessions(data);
            case "FETCHCASH":
                return RequestCommands.fetchCash(data);
            case "FETCHSTATS":
                return RequestCommands.fetchStats(data);
            case "GENERATE_FOOD_ITEM":
                return RequestCommands.generateFoodItem(data);
            case "GENERATE_CLOTHING_ITEM":
                return RequestCommands.generateClothingItem(data);
            case "FETCH_FOOD_ITEM":
                return RequestCommands.fetchFood(data);
            case "FETCH_CLOTHING_ITEMS":
                return RequestCommands.fetchClothing(data);
            case "USERELAX":
                return RequestCommands.useRelax(data);
            case "STARTWORK":
                return RequestCommands.startWork(data);
            case "CANCELWORK":
                return RequestCommands.cancelWork(data);
            case "CHECKWORK":
                return RequestCommands.checkWork(data);
            case "STARTTRAINING":
                return RequestCommands.startTraining(data);
            case "STOPTRAINING":
                return RequestCommands.stopTraining(data);
            case "FETCHTRAINING":
                return RequestCommands.fetchTraining(data);
            default:
                return "ERROR UnknownCommand";
        }
    }
}
