
import requests

API_URL = "http://localhost:5000/api/clients"  
def get_clients():
    response = requests.get(API_URL)
    if response.status_code == 200:
        clients = response.json()
        for client in clients:
            print(f"ID: {client['id']}, Name: {client['name']}")
    else:
        print("Error:", response.status_code)

if __name__ == "__main__":
    get_clients()