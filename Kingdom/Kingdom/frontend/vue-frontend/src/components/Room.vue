<script setup>
import { ref, watch } from "vue";
import Forest from "../../public/assets/Forest.jpg";
import Plains from "../../public/assets/Plains.jpg";
import Mountain from "../../public/assets/Mountain.jpg";
import Swamp from "../../public/assets/Swamp.jpg";

const props = defineProps({
  roomName: String,
  numberOfPlayers: Number,
  playerCount: Number,
  role: String,
});

let showRole = ref(false);
let roleImage = ref("");
let roleDescription = ref("");
const updateRole = (role) => {
  showRole.value = true;
  switch (role) {
    case "King":
      roleImage.value = Plains;
      roleDescription.value =
        "The King/Queen (Plains) - Starts with a bonus 25% life (e.g 50 in EDH). The King/Queen's goal is to be the last person standing, removing other players through convention means for a game of Magic to the appropriate format ('21 commander damage' rule applies in EDH Kingdoms). The King/Queen is the only player who reveals their role card, which they must do before the first round of play.";
      break;
    case "Knight":
      roleImage.value = Forest;
      roleDescription.value =
        "The Knight (Forest) - The shield of the King/Queen. The Knight's goal is to keep the King/Queen alive by any means necessary, even if it means sacrificing themself. If the King/Queen wins the game, the Knight also wins the game, even if they have been removed. The Knight plays as a standard player with no special privileges, meaning they cannot block for the King/Queen with their creatures, share mana or cards in hand, or perform any table-top maneuver's that would otherwise be illegal in standard Magic play.";
      break;
    case "Bandit":
      roleImage.value = Mountain;
      roleDescription.value =
        "The Bandits (Mountains) - The only role of which there are two. The Bandits are a team whose goal is simply to kill the King/Queen. If the King/Queen dies at any point (and not to the hands of the Usurper or the final kill of the Assassin) then both Bandits win regardless of their status. Although they are on the same team, the Bandits have no way of guaranteeing who their fellow Bandit is. Similarly to the relationship with the King/Queen and the Knight, the Bandits share no special rules with each other, only the win condition.";
      break;
    case "Usurper":
      roleImage.value = "../src/assets/Island.jpg";
      roleDescription.value =
        "The Usurper (Island - only played with six people) - The Usurper's goal is to become the King/Queen. If the Usurper manages to deliver the killing blow to the current King/Queen, they reveal their role card. Instead of dying, the original King/Queen is reduced to 1 life and acquires the Usurper role card. The Usurper's life total then becomes that which the King/Queen started with (even if that means losing life) and the Usurper becomes the new King/Queen. The Knight's allegiance changes immediately, as does the Bandits' target. The old King/Queen becomes the new Usurper. Usurpers kill other players normally and gain no special rules for interacting with them.";
      break;
    case "Assassin":
      roleImage.value = Swamp;
      roleDescription.value =
        "The Assassin (Swamp) - The Assassin has one of the trickier goals; to be the last person standing. Although this is functionally identical to the King/Queen, the Assassin must be careful as to when they target each player. Kill the Knight too early and you risk the Bandits taking out the King/Queen. Kill the King/Queen too early and the Bandits win. No other special rules apply.";
      break;
    default:
      roleImage.value = "";
      roleDescription.value = "No Role";
      break;
  }
};

// Watch for changes in the role prop
watch(
  () => props.role,
  (newRole) => {
    updateRole(newRole);
  }
);

// Initialize the role if it is already set
updateRole(props.role);
</script>

<template>
  <p>Room Name {{ roomName }}</p>
  <p>Number of Players {{ playerCount }}</p>

  <div v-if="showRole" class="JoinedRoom">
    <img :src="roleImage" alt="Role Image" class="roleImage" />
    <p>{{ roleDescription }}</p>
  </div>
</template>
