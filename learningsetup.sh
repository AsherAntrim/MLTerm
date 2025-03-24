py -m venv .
./Scripts/activate
pip install torch~=2.2.1 --index-url https://download.pytorch.org/whl/cu121
pip install ./ml-agents/ml-agents-envs/
pip install ./ml-agents/ml-agents/
