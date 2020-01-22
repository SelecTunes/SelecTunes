package com.ncdc.project;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.ncdc.project.flights.Flight;
import com.ncdc.project.passengers.Passenger;
import com.ncdc.project.repository.FlightRepositoryWrapper;
import com.ncdc.project.repository.PassengerRepositoryWrapper;
import com.ncdc.project.repository.UserRepositoryWrapper;
import org.json.simple.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.List;

@Controller
public class AppController {

    private FlightRepositoryWrapper flightRepositoryWrapper;

    private PassengerRepositoryWrapper passengerRepositoryWrapper;

    private UserRepositoryWrapper userRepositoryWrapper;

    @Autowired
    public void setUserRepositoryWrapper(UserRepositoryWrapper userRepositoryWrapper) {
        this.userRepositoryWrapper = userRepositoryWrapper;
    }

    @Autowired
    public void setPassengerRepositoryWrapper(PassengerRepositoryWrapper passengerRepositoryWrapper) {
        this.passengerRepositoryWrapper = passengerRepositoryWrapper;
    }

    @Autowired
    public void setFlightRepositoryWrapper(FlightRepositoryWrapper flightRepositoryWrapper) {
        this.flightRepositoryWrapper = flightRepositoryWrapper;
    }

    @RequestMapping(path="/", method=RequestMethod.GET)
    public String indexView(Model model) {
        model.addAttribute("flights", flightRepositoryWrapper.findAll());
        model.addAttribute("passengernum", passengerRepositoryWrapper.findAll().size());
        return "index";
    }

    @RequestMapping(path="/purchase", method = RequestMethod.GET)
    public String purchaseView(Model model) {
        List<Flight> flights = flightRepositoryWrapper.findAll();
        model.addAttribute("passenger", new Passenger());
        model.addAttribute("flights", flights);
        return "purchase";
    }

    @RequestMapping(path="/purchase", method = RequestMethod.POST)
    public String purchaseTicket(@ModelAttribute("passenger") Passenger passenger) {
        passengerRepositoryWrapper.addPassenger(passenger);
        sendToAlex(passengerRepositoryWrapper.findPassengerByName(passenger.getName()));
        return "redirect:success";
    }

    @RequestMapping(path="/success", method = RequestMethod.GET)
    public String successView() {
        return "success";
    }

    @RequestMapping(path="/admin", method=RequestMethod.GET)
    public String adminView(Model model) {
        return "admin";
    }

    private void sendToAlex(Passenger passenger) {
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
