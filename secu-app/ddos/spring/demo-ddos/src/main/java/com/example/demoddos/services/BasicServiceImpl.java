package com.example.demoddos.services;

import com.example.demoddos.exceptions.CustomTooManyRequestException;
import com.example.demoddos.models.CommonType;
import com.example.demoddos.models.FailureMessage;
import com.example.demoddos.models.SuccessMessage;
import io.github.resilience4j.ratelimiter.RequestNotPermitted;
import io.github.resilience4j.ratelimiter.annotation.RateLimiter;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class BasicServiceImpl {

    @RateLimiter(name = "basic-service", fallbackMethod = "fallback")
    public CommonType getData() {
        // Ici, on doit normalement faire toute la logique permettant d'obtenir des données provenant d'une autre API ou directement de la BdD

        return SuccessMessage.builder()
                .message("Success")
                .data(List.of("Element A", "Element B", "Element C"))
                .build();
    }

    public CommonType fallback(RequestNotPermitted requestNotPermitted) {
        throw new CustomTooManyRequestException("Too Many Requests");
//        return FailureMessage.builder()
//                .content("Failure")
//                .build();
    }


}
