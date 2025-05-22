package com.example.demoddos.models;

import lombok.Builder;
import lombok.Data;

import java.util.List;

@Data
@Builder
public class SuccessMessage implements CommonType {
    private String message;
    private List<String> data;

}
