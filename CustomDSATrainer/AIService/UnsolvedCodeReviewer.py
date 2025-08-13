import os
from google import genai
from dotenv import load_dotenv

BASE_DIR = os.path.dirname(os.path.abspath(__file__))

load_dotenv()

gemini_api_key = os.getenv("API_KEY")
client = genai.Client(api_key=gemini_api_key)

prompt_path = os.path.join(BASE_DIR, 'CodeReview/Prompts/Prompt-Unsolved.txt')
with open(prompt_path, 'r', encoding='utf-8') as file:
    question = file.read()

response = client.models.generate_content(
    model="gemini-2.0-flash",
    contents=question
)

text = response.text
print(text)

resultPath = "CodeReview/Result.txt"
with open(resultPath, 'w') as file:
    file.write(text)