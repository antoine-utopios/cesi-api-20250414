package com.example.demoddos.controllers;

import com.example.demoddos.models.CommonType;
import com.example.demoddos.services.BasicServiceImpl;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/test")
public class BasicController {
    private final BasicServiceImpl basicService;

    @GetMapping
    public ResponseEntity<CommonType> getCommonType() {
        return ResponseEntity.ok(basicService.getData());
    }
}
