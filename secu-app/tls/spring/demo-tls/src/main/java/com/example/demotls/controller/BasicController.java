package com.example.demotls.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/v1")
public class BasicController {
    @GetMapping("/test")
    public String test(){
        return "Tout s'est bien passé !";
    }
}
