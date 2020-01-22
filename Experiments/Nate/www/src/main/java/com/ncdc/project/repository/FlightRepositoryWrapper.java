package com.ncdc.project.repository;

import com.ncdc.project.flights.Flight;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Set;

@Service
public class FlightRepositoryWrapper {

    private FlightRepository flightRepository;

    @Autowired
    public void setFlightRepository(FlightRepository flightRepository) {
        this.flightRepository = flightRepository;
    }

    public List<Flight> findAll() {
        return flightRepository.findAll();
    }

    @Transactional
    public void save(Flight toAdd) {
        flightRepository.save(toAdd);
    }

    @Transactional
    public void saveAll(Set<Flight> newFlights) {
        newFlights.forEach(this::save);
    }
}
