Feature: Weather forecast

    Scenario: Get weather forecast single day
        Given I have a weather forecast
          | Date       | TemperatureC | Summary |
          | 2021-01-01 | 20           | Sunny   |
        When I request the weather forecast
        Then I should receive a weather forecast
          | Date       | TemperatureC | Summary |
          | 2021-01-01 | 20           | Sunny   |
          
    Scenario: Get weather forecast
        Given I have a weather forecast
          | Date       | TemperatureC | Summary |
          | 2021-01-01 | 20           | Sunny   |
          | 2021-01-02 | 21           | Cloudy  |
          | 2021-01-03 | 22           | Rainy   |
          | 2021-01-04 | 23           | Snowy   |
          | 2021-01-05 | 24           | Windy   |
        When I request the weather forecast
        Then I should receive a weather forecast
          | Date       | TemperatureC | Summary |
          | 2021-01-01 | 20           | Sunny   |
          | 2021-01-02 | 21           | Cloudy  |
          | 2021-01-03 | 22           | Rainy   |
          | 2021-01-04 | 23           | Snowy   |
          | 2021-01-05 | 24           | Windy   |
          