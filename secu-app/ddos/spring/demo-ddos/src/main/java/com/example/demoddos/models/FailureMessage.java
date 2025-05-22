package com.example.demoddos.models;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class FailureMessage implements CommonType {
    private String content;
}
