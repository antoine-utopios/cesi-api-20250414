# Exercice Sécurité des Application - 1

## Objectifs 

Appréhender la protection des applications contre les attaques de type DDoS et le Brute Force.

## Sujet

* Réaliser une API, dans le langage de votre choix, permettant à des utilisateurs de se connecter (avec la couche de sécurité adapté et les design pattern optimaux). 
* Cette API devra permettre aux utilisateurs de s'enregistrer / se connecter et de se voir retourner un JWT en cas de connexion réussie.
* L'API devra être résiliente aux attaques de type DDoS et retourner un `429 - Too Many Requests` en cas de ce genre d'attaque au lieu de crash. 
* L'API devra aussi informer les utilisateurs en cas de compte vérrouillé via un message de retour dédié et le code statut adapté.
* BONUS: L'API devra envoyer un log à une autre API (simulation d'architecture en micro-services), par une requête HTTP, de sorte à lui demander de stocker qu'un compte est désormais verrouillé (en base de données ou dans un fichier, au choix).