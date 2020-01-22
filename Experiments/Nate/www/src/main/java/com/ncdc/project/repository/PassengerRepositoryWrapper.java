package com.ncdc.project.repository;

import com.ncdc.project.passengers.Passenger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class PassengerRepositoryWrapper {

    private PassengerRepository passengerRepository;

    @Autowired
    public void setFlightRepository(PassengerRepository passengerRepository) {
        this.passengerRepository = passengerRepository;
    }

    public List<Passenger> findAll() {
        return passengerRepository.findAll();
    }

    public Passenger findPassengerByName(String name) {
        return passengerRepository.findPassengerByName(name);
    }

    @Transactional
    public void addPassenger(Passenger toAdd) {
        passengerRepository.save(toAdd);
    }
}
