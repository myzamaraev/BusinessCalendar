<template>
  <create-calendar-dialog
    v-if="isCreateMode"
    @cancelled="isCreateMode = false"
    @submitted="onCreateCalendarSubmitted"
  ></create-calendar-dialog>
  <table cellspacing="0">
    <thead>
      <tr>
        <th>Identifier</th>
        <th>Type</th>
        <th>Key</th>
        <th></th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="calendar in calendars" :key="calendar.id">
        <td>{{ calendar.id }}</td>
        <td>{{ calendar.type }}</td>
        <td>{{ calendar.key }}</td>
        <td>
          <router-link :to="calendarRoute(calendar)">Open</router-link>
        </td>
      </tr>
      <tr>
        <td colspan="4" @click="loadMore">... Load more</td>
      </tr>
      <tr>
        <td colspan="4" @click="isCreateMode = true">+ Add calendar</td>
      </tr>
    </tbody>
  </table>
</template>

<script>
import CreateCalendarDialog from "./CreateCalendarDialog.vue";
export default {
  components: { CreateCalendarDialog },
  name: "calendars-list",
  data() {
    return {
      isCreateMode: false,
      page: 0,
      calendars: [],
    };
  },
  methods: {
    calendarRoute(calendar) {
      return {
        name: "calendar",
        params: {
          calendarType: calendar.type,
          calendarKey: calendar.key,
        },
      };
    },
    loadMore() {
      this.page++;
      this.loadIdentifiers();
    },
    loadIdentifiers() {
      var url = new URL("/CalendarIdentifier/List", window.location.origin);
      url.searchParams.append("page", this.page);
      url.searchParams.append("pageSize", 5);

      fetch(url)
        .then((response) => {
          if (!response.ok) {
            throw new Error("request failed! Status: " + response.status);
          }

          return response.json();
        })
        .then((data) => {
          this.calendars.push.apply(this.calendars,data);
        })
        .catch((error) => {
          console.log(error);
        });
    },
    onCreateCalendarSubmitted(type, key) {
      this.createCalendarIdentifier(type, key);
      this.isCreateMode = false;
    },
    createCalendarIdentifier(type, key) {
      const url = new URL("/CalendarIdentifier", window.location.origin);
      fetch(url, {
        method: "Post",
        headers: {
          "Content-type": "application/json",
        },
        body: JSON.stringify({ type, key }),
      }).then((response) => {
        if (!response.ok) {
          throw new Error("request failed! Status: " + response.status);
        }
        this.page = 0;
        this.loadIdentifiers();
      });

    },
  },
  mounted() {
    this.loadIdentifiers();
  },
};
</script>

<style scoped>
table {
  border-collapse: collapse;
  width: 90%;
  margin: auto;
  border: 2px solid;
}

th {
  padding: 8px;
  background-color: rgb(185, 200, 247);
}

td {
  padding: 8px 10px;
}

tr:nth-child(even) {
  background-color: rgb(243, 243, 243);
}

tr:hover {
  font-weight: bold;
  background-color: rgb(224, 255, 245);
}
</style>
