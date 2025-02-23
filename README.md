
# **[Secret Santa]**
 

## **📖 Overview**
The Secret Santa App is a web application designed to facilitate anonymous gift exchange among employees. It randomly pairs participants while ensuring fairness and uniqueness in the pair selection process. 🎄🎁

✨ Features

✔ Random Pairing – Employees are matched using a randomized algorithm.

✔ Fairness Rules – Self-pairing is prevented, and each employee is both a giver and a receiver.

✔ Unique Pairs – No duplicate pairings occur within a single run.

✔ Dynamic Generation – Each new draw produces a different pairing combination.

✔ User-Friendly Report – A clear and structured output lists all Secret Santa pairs.

✔ User Wishlist – Users can see their pair's wishlist.


---

## **🛠️ Technologies Used**
- **Frontend:** [React.js]  
- **Backend:** [.NET]  



---



## **📝 Getting Started**
Follow these steps to run the project locally:

1. Clone the repository:  
   ```bash
   git clone https://github.com/asmer085/secret_santa
   ```
2. Navigate to the project directory:  
   ```bash
   cd secret_santa
   ```
3. Start the application using Docker:  
   ```bash
   docker-compose up --build
   ```
4. Open `http://localhost:3000` in your browser.

## **⚠️	Important Note:**
When running the application via Docker, you may encounter an issue where the frontend cannot access backend endpoints. 
This happens because the browser blocks requests to the backend due to the use of a self-signed certificate (HTTPS). 
To resolve this:

Open **https://localhost:44394/swagger/index.html** in your browser.

When prompted, allow access to the self-signed certificate. This will enable the browser to trust the backend.

If the issue persists, restart the Docker containers by running:

```bash
docker-compose down
docker-compose up --build
```

This issue occurs because the backend uses HTTPS with self-signed certificates (aspnetapp.crt and .pfx files), 
which are not automatically trusted by browsers or operating systems (e.g., Linux).

---

## **📧 Contact**
For inquiries or feedback:  
📩 Email: (asmerkarabeg61@gmail.com)  
🔗 LinkedIn: (https://www.linkedin.com/in/asmer-karabeg-6ba619269/)  
