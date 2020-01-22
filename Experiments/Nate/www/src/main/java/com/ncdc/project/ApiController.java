package com.ncdc.project;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.ncdc.project.flights.Flight;
import com.ncdc.project.passengers.Passenger;
import com.ncdc.project.repository.FlightRepositoryWrapper;
import com.ncdc.project.repository.PassengerRepositoryWrapper;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.net.URI;
import java.util.List;

@RestController
@RequestMapping("/api")
public class ApiController {

    private FlightRepositoryWrapper flightRepositoryWrapper;

    private PassengerRepositoryWrapper passengerRepositoryWrapper;

    @Autowired
    public void setFlightRepositoryWrapper(FlightRepositoryWrapper flightRepositoryWrapper) {
        this.flightRepositoryWrapper = flightRepositoryWrapper;
    }

    @Autowired
    public void setPassengerRepositoryWrapper(PassengerRepositoryWrapper passengerRepositoryWrapper) {
        this.passengerRepositoryWrapper = passengerRepositoryWrapper;
    }

    @GetMapping("/passengers")
    @ResponseBody
    public String getAllPassengers() {
        JSONArray toReturn = new JSONArray();
        try {
            List<Passenger> passengerList = passengerRepositoryWrapper.findAll();


            for (Passenger passenger : passengerList) {
                JSONObject passengerToAdd = createFromPassenger(passenger);
                toReturn.add(passengerToAdd);
            }
            return toReturn.toJSONString();
        } catch (Exception e) {
            System.out.println("call to /api/passengers failed, probably no passengers yet");
            e.printStackTrace();
            return toReturn.toJSONString();
        }
    }

    @GetMapping("flights")
    @ResponseBody
    public String getAllFlights() {
        try {
            List<Flight> flightList = flightRepositoryWrapper.findAll();
            JSONArray toReturn = new JSONArray();

            for (Flight flight : flightList) {
                JSONObject flightToAdd = new JSONObject();
                flightToAdd.put("source", flight.getSource());
                flightToAdd.put("destination", flight.getDestination());
                flightToAdd.put("arrival", flight.getArrival());
                flightToAdd.put("departure", flight.getDeparture());
                flightToAdd.put("uid", flight.getUid());
                toReturn.add(flightToAdd);
            }
            return toReturn.toJSONString();
        } catch (Exception e) {
            System.out.println("call to /api/flights failed, probably no flights yet");
            e.printStackTrace();
            return null;
        }
    }

    @RequestMapping(
            path="/passengers/purchase",
            method=RequestMethod.POST)
    @ResponseStatus(HttpStatus.CREATED)
    @ResponseBody
    public void postCustomer(@RequestBody Passenger passenger){
        Passenger toAdd = new Passenger(passenger);
        passengerRepositoryWrapper.addPassenger(toAdd);
        sendToAlex(passengerRepositoryWrapper.findPassengerByName(passenger.getName()));
    }

    @RequestMapping(
            path="flights/add",
            method=RequestMethod.POST)
    @ResponseStatus(HttpStatus.CREATED)
    @ResponseBody
    public void addFlight(@RequestBody Flight flight) {
        Flight toAdd = new Flight(flight);
        flightRepositoryWrapper.save(toAdd);
    }

    public void sendToAlex(Passenger passenger) {
        try {
            ObjectMapper objectMapper = new ObjectMapper();
            String requestBody = objectMapper
                    .writeValueAsString(createFromPassenger(passenger));

            HttpClient client = HttpClient.newHttpClient();
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create("http://10.0.115.114/api/ticket"))
                    .POST(HttpRequest.BodyPublishers.ofString(requestBody))
                    .setHeader("Content-type", "application/json")
                    .build();

            client.send(request, HttpResponse.BodyHandlers.ofString());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private JSONObject createFromPassenger(Passenger passenger) {
        JSONObject passengerToAdd = new JSONObject();
        passengerToAdd.put("uid", passenger.getId());
        passengerToAdd.put("username", passenger.getUsername());
        passengerToAdd.put("address", passenger.getAddress());
        passengerToAdd.put("birth_date", passenger.getBirthdate());
        passengerToAdd.put("blood_group", passenger.getBlood_group());
        passengerToAdd.put("company", passenger.getCompany());
        passengerToAdd.put("credit_card_expire", passenger.getCredit_card_expire());
        passengerToAdd.put("credit_card_number", passenger.getCredit_card_number());
        passengerToAdd.put("credit_card_provider", passenger.getCredit_card_provider());
        passengerToAdd.put("credit_card_security_code", passenger.getCredit_card_security_code());
        passengerToAdd.put("job", passenger.getJob());
        passengerToAdd.put("mail", passenger.getMail());
        passengerToAdd.put("name", passenger.getName());
        passengerToAdd.put("residence", passenger.getResidence());
        passengerToAdd.put("sex", passenger.getSex());
        passengerToAdd.put("ssn", passenger.getSsn());
        passengerToAdd.put("flight_id", passenger.getFlightid());
//        passengerToAdd.put("flight_id", "48928");
        passengerToAdd.put("through_security", false);
        passengerToAdd.put("is_checked_in", false);
        passengerToAdd.put("has_baggage", false);
        passengerToAdd.put("baggage_checked", false);
        return passengerToAdd;
    }

}
