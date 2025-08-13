import os
from google import genai
from dotenv import load_dotenv

BASE_DIR = os.path.dirname(os.path.abspath(__file__))

load_dotenv()

gemini_api_key = os.getenv("API_KEY")
client = genai.Client(api_key=gemini_api_key)

prompt_path = os.path.join(BASE_DIR, 'ProblemLLMPrompt.txt')
with open(prompt_path, 'r', encoding='utf-8') as file:
    question = file.read()

response = client.models.generate_content(
    model="gemini-2.0-flash",
    contents=question
)

lines = response.text.splitlines()

inputStarted = False
outputStarted = False
hintFound = False
statement = ""
input_data = ""
output_data = ""
hint = ""
currentInput = 1
currentOutput = 1

for line in lines:
    if not hintFound:
        if line == 'HINT':
            hintFound = True
            continue
        else:
            statement += line + '\n'
    elif not inputStarted:
        if line == 'INPUT':
            inputStarted = True
            continue
        else:
            hint += line
    elif not outputStarted:
        if line == 'OUTPUT':
            outputStarted = True
            input_file_path = os.path.join(BASE_DIR, "Task", "Inputs", f"{currentInput}.txt")
            os.makedirs(os.path.dirname(input_file_path), exist_ok=True)
            with open(input_file_path, 'w') as f:
                f.write(input_data)
            continue
        else:
            if line == '!':
                input_file_path = os.path.join(BASE_DIR, "Task", "Inputs", f"{currentInput}.txt")
                os.makedirs(os.path.dirname(input_file_path), exist_ok=True)
                with open(input_file_path, 'w') as f:
                    f.write(input_data)
                input_data = ""
                currentInput += 1
            else:
                input_data += line + '\n'
    else:
        if line == '!':
            output_file_path = os.path.join(BASE_DIR, "Task", "Outputs", f"{currentOutput}.txt")
            os.makedirs(os.path.dirname(output_file_path), exist_ok=True)
            with open(output_file_path, 'w') as f:
                f.write(output_data)
            output_data = ""
            currentOutput += 1
        else:
            output_data += line + '\n'

output_file_path = os.path.join(BASE_DIR, "Task", "Outputs", f"{currentOutput}.txt")
os.makedirs(os.path.dirname(output_file_path), exist_ok=True)
with open(output_file_path, 'w') as f:
    f.write(output_data)

statement_path = os.path.join(BASE_DIR, 'Task', 'Statement.txt')
os.makedirs(os.path.dirname(statement_path), exist_ok=True)
with open(statement_path, 'w') as statementFile:
    statementFile.write(statement)

hint_path = os.path.join(BASE_DIR, 'Task', 'Hint.txt')
os.makedirs(os.path.dirname(hint_path), exist_ok=True)
with open(hint_path, 'w') as hintFile:
    hintFile.write(hint)