package com.ncdc.project.repository;

import com.ncdc.project.passengers.Passenger;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PassengerRepository extends JpaRepository<Passenger, Integer> {
    List<Passenger> findAll();
    Passenger findPassengerByName(String name);
}