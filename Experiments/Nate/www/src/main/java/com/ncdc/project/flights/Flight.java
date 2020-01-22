package com.ncdc.project.flights;

import javax.persistence.*;
import java.io.Serializable;

@Entity
@Table(name="flights")
public class Flight implements Serializable {
    private static final long serialVersionUID = -2343243243242432341L;
    @Id
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator ="SEQ_GEN")
    @SequenceGenerator(name="my_entity_sequence_gen", sequenceName = "MY_ENTITY_SEQ")
    private long id;

    @Column(name="uid")
    private Integer uid;

    @Column(name="arrival")
    private String arrival;

    @Column(name="departure")
    private String departure;

    @Column(name="source")
    private String source;

    @Column(name="destination")
    private String destination;

    public Flight() {};

    public Flight(Flight flight) {
        this.uid = flight.uid;
        this.arrival = flight.arrival;
        this.departure = flight.departure;
        this.source = flight.source;
        this.destination = flight.destination;
    }

    public Integer getUid() {
        return uid;
    }

    public String getArrival() {
        return arrival;
    }

    public String getDeparture() {
        return departure;
    }

    public String getSource() {
        return source;
    }

    public String getDestination() {
        return destination;
    }

    public boolean equals(Flight flight) {
        return (this.uid.equals(flight.uid) && this.destination.equals(flight.destination) && this.source.equals(flight.source) && this.departure.equals(flight.departure) && this.arrival.equals(flight.arrival));
    }
}
