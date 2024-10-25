function addToCalendar(title, eventDate, location = "", description = "") {
  console.log("title", title);
  console.log("eventDate", eventDate);
  console.log("location", location);
  console.log("description", description);

  const formattedDate = new Date(eventDate)
    .toISOString()
    .replace(/-|:|\.\d+/g, "");

  window.open(
    "https://calendar.google.com/calendar/render?action=TEMPLATE&text=" +
      title +
      "&location=" +
      location +
      "&details=" +
      description +
      "&dates=" +
      formattedDate +
      "/" +
      formattedDate,
    "_blank"
  );
}
