package com.ncdc.project.users;

import javax.persistence.*;

@Entity
@Table(name="users")
public class AirportUser {
    private static final long serialVersionUID = -2343243243242432341L;
    @Id
    @GeneratedValue(strategy= GenerationType.SEQUENCE, generator ="SEQ_GEN")
    @SequenceGenerator(name="my_entity_sequence_gen", sequenceName = "MY_ENTITY_SEQ")
    private long id;

    @Column(name="name")
    private String name;

    @Column(name="username")
    private String username;

    @Column(name="is_admin")
    private boolean isAdmin;

    @Column(name="email")
    private String email;

    @Column(name="password")
    private String password;

    public AirportUser () {};

    public AirportUser(AirportUser user) {
        this.email = user.email;
        this.isAdmin = user.isAdmin;
        this.name = user.name;
        this.username = user.username;
        this.password = user.password;
    }

    public AirportUser(String name, String username, boolean isAdmin, String email, String password) {
        this.name = name;
        this.username = username;
        this.isAdmin = isAdmin;
        this.email = email;
        this.password = password;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public boolean isAdmin() {
        return isAdmin;
    }

    public void setAdmin(boolean admin) {
        isAdmin = admin;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}
