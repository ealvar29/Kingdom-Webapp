<script setup>
import { ref } from "vue";
const weatherData = ref(null);
fetch("https://localhost:7090/WeatherForecast")
  .then((response) => response.json())
  .then((data) => (weatherData.value = data));
</script>

<template>
  <div class="card m-3">
    <h5 class="card-header text-center">Vue 3 fetch data from API</h5>
    <div class="card-body">
      <div v-if="weatherData">
        <h5>Weather</h5>
        <ul class="mb-0">
          <li v-for="weather in weatherData" :key="weather.date">
            {{ weather.date }}, {{ weather.temperatureC }},
            {{ weather.temperatureF }}
            {{ weather.summary }}
          </li>
        </ul>
      </div>
      <div v-if="!products" class="text-center">
        <div class="spinner-border spinner-border-sm"></div>
      </div>
    </div>
  </div>
</template>
