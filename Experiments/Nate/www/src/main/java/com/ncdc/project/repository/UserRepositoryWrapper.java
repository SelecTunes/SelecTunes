package com.ncdc.project.repository;

import com.ncdc.project.users.AirportUser;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class UserRepositoryWrapper {
    private UserRepository userRepository;

    @Autowired
    private void setUserRepository(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    public AirportUser findAirportUserByUsername(String name) {
        return userRepository.findAirportUserByUsername(name);
    }

    public List<AirportUser> findAll() {
        return userRepository.findAll();
    }

    @Transactional
    public void addUser(AirportUser user) {
        this.userRepository.save(user);
    }
}
