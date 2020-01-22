package com.ncdc.project.tasks;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.ncdc.project.flights.Flight;
import com.ncdc.project.repository.FlightRepositoryWrapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.HashSet;
import java.util.List;
import java.util.Set;


@Component
public class Fetcher {

    FlightRepositoryWrapper flightRepositoryWrapper;

    @Autowired
    public void setFlightRepositoryWrapper(FlightRepositoryWrapper flightRepositoryWrapper) {
        this.flightRepositoryWrapper = flightRepositoryWrapper;
    }

    @Scheduled(fixedRate = 5000)
    public void fetchFlightsFromAlex() {
        try {
            URL url = new URL("http://10.0.115.114/api/flights");
            HttpURLConnection con = (HttpURLConnection) url.openConnection();
            BufferedReader in = new BufferedReader(
                    new InputStreamReader(
                            con.getInputStream()));
            String inputLine;

            while ((inputLine = in.readLine()) != null)
                convertAndAddToDB(inputLine);
            in.close();
        } catch (Exception ignored) {

        }
    }

    public void convertAndAddToDB(String response) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            // Get a Set of flights from the response, a String, mapped to Flight objects
            Set<Flight> newFlights = new HashSet<>(mapper.readValue(response, new TypeReference<List<Flight>>() {
            }));

            // Get all of the flights already in the repository
            Set<Flight> old = new HashSet<>(flightRepositoryWrapper.findAll());

            for (Flight flight : newFlights) {
                if (!contains(old, flight)) flightRepositoryWrapper.save(flight);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private boolean contains(Set<Flight> oldFlights, Flight compareTo) {
        for (Flight oldFlight : oldFlights) {
            if (oldFlight.equals(compareTo)) return true;
        }
        return false;
    }
}
