import os
import sys
from google import genai
from dotenv import load_dotenv

BASE_DIR = os.path.dirname(os.path.abspath(__file__))

load_dotenv()

gemini_api_key = os.getenv("API_KEY")
client = genai.Client(api_key=gemini_api_key)

prompt_path = os.path.join(BASE_DIR, 'CodeReview/Templates/Unsolved.txt')
with open(prompt_path, 'r', encoding='utf-8') as file:
    question = file.read()

all_input = sys.stdin.read()
parts = all_input.split("----------")
problemStatement = parts[0];
userSource = parts[1]

lines = question.splitlines()

lines[3] = problemStatement
lines[6] = userSource

question = ''.join(lines);

response = client.models.generate_content(
    model="gemini-2.0-flash",
    contents=question
)

text = response.text
print(text, flush = True)
