package com.example.demoxss;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.header.writers.XXssProtectionHeaderWriter;
import org.springframework.security.web.server.header.XXssProtectionServerHttpHeadersWriter;

@Configuration
@EnableWebSecurity
public class WebSecurityConfig {

    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http
                .headers(headers ->
                    headers.xssProtection(xss ->
                            xss
                                    // On configure l'utilisation du Header pour avertir les navigateurs
                                    .headerValue(XXssProtectionHeaderWriter.HeaderValue.ENABLED_MODE_BLOCK))

                                    // ON n'autorise que les scripts qui proviendrait de la même source que le serveur d'où provient la page HTML où s'y trouveraient des scripts
                                    .contentSecurityPolicy(cps -> cps.policyDirectives("script-scr 'self'")
                            )
                )
                .authorizeHttpRequests(requests -> requests.anyRequest().permitAll());

        return http.build();
    }
}
