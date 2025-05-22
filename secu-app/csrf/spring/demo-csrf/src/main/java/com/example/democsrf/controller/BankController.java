package com.example.democsrf.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
@RequestMapping("/bank")
public class BankController {
    @GetMapping("/transfer")
    public String transfer() {
        return "create-transfer";
    }

    @PostMapping("/transfer")
    public String transfer(@RequestParam Integer accountNo, @RequestParam Double amount) {
        System.out.println("Transfer from : " + accountNo + " with amount : " + amount);

        return "validate-transfer";
    }

}
