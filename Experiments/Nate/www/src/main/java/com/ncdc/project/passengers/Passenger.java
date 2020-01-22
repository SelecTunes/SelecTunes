package com.ncdc.project.passengers;

import javax.persistence.*;
import java.io.Serializable;

@Entity
@Table(name="passengers")
public class Passenger implements Serializable {
    private static final long serialVersionUID = -2343243243242432341L;
    @Id
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator ="SEQ_GEN")
    @SequenceGenerator(name="my_entity_sequence_gen", sequenceName = "MY_ENTITY_SEQ")
    private long id;

    @Column(name="address")
    private String address;

    @Column(name="birthdate")
    private String birthdate;

    @Column(name="blood_group")
    private String blood_group;

    @Column(name="company")
    private String company;

    @Column(name="credit_card_expire")
    private String credit_card_expire;

    @Column(name="credit_card_number")
    private String credit_card_number;

    @Column(name="credit_card_provider")
    private String credit_card_provider;

    @Column(name="credit_card_security_code")
    private String credit_card_security_code;

    @Column(name="job")
    private String job;

    @Column(name="mail")
    private String mail;

    @Column(name="name")
    private String name;

    @Column(name="residence")
    private String residence;

    @Column(name="sex")
    private String sex;

    @Column(name="ssn")
    private String ssn;

    @Column(name="username")
    private String username;

    @Column(name="flightid")
    private String flightid;

    public Passenger(){};

    public Passenger(Passenger passenger) {
        this.address = passenger.address;
        this.birthdate = passenger.birthdate;
        this.blood_group = passenger.blood_group;
        this.company = passenger.company;
        this.credit_card_expire = passenger.credit_card_expire;
        this.credit_card_number = passenger.credit_card_number;
        this.credit_card_provider = passenger.credit_card_provider;
        this.credit_card_security_code = passenger.credit_card_security_code;
        this.job = passenger.job;
        this.mail = passenger.mail;
        this.name = passenger.name;
        this.residence = passenger.residence;
        this.sex = passenger.sex;
        this.ssn = passenger.ssn;
        this.username = passenger.username;
        this.flightid = passenger.flightid;
    }

    public long getId() {
        return id;
    }

    public String getAddress() {
        return address;
    }

    public String getBirthdate() {
        return birthdate;
    }

    public String getBlood_group() {
        return blood_group;
    }

    public String getCompany() {
        return company;
    }

    public String getCredit_card_expire() {
        return credit_card_expire;
    }

    public String getCredit_card_number() {
        return credit_card_number;
    }

    public String getCredit_card_provider() {
        return credit_card_provider;
    }

    public String getCredit_card_security_code() {
        return credit_card_security_code;
    }

    public String getJob() {
        return job;
    }

    public String getMail() {
        return mail;
    }

    public String getName() {
        return name;
    }

    public String getResidence() {
        return residence;
    }

    public String getSex() {
        return sex;
    }

    public String getSsn() {
        return ssn;
    }

    public String getUsername() {
        return username;
    }

    public String getFlightid() {
        return flightid;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public void setBirthdate(String birthdate) {
        this.birthdate = birthdate;
    }

    public void setBlood_group(String blood_group) {
        this.blood_group = blood_group;
    }

    public void setCompany(String company) {
        this.company = company;
    }

    public void setCredit_card_expire(String credit_card_expire) {
        this.credit_card_expire = credit_card_expire;
    }

    public void setCredit_card_number(String credit_card_number) {
        this.credit_card_number = credit_card_number;
    }

    public void setCredit_card_provider(String credit_card_provider) {
        this.credit_card_provider = credit_card_provider;
    }

    public void setCredit_card_security_code(String credit_card_security_code) {
        this.credit_card_security_code = credit_card_security_code;
    }

    public void setJob(String job) {
        this.job = job;
    }

    public void setMail(String mail) {
        this.mail = mail;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setResidence(String residence) {
        this.residence = residence;
    }

    public void setSex(String sex) {
        this.sex = sex;
    }

    public void setSsn(String ssn) {
        this.ssn = ssn;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public void setFlightid(String flightid) {
        this.flightid = flightid;
    }

    @Override
    public String toString() {
        return address + blood_group + birthdate + credit_card_security_code + credit_card_provider + credit_card_number + credit_card_expire + residence + ssn + sex + username + mail + job;
    }
}