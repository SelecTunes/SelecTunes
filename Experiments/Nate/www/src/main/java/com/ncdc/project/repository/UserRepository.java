package com.ncdc.project.repository;

import com.ncdc.project.users.AirportUser;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface UserRepository extends JpaRepository<AirportUser, Integer> {
    AirportUser findAirportUserByUsername(String username);
    List<AirportUser> findAll();
}
