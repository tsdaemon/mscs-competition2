import pandas as pd
import codecs as cd


def json_to_pandas(json_path):
	# read the entire file into a python array
	with cd.open(json_path, 'r', 'utf-8') as f:
		data = f.readlines()

	# remove the trailing "\n" from each line
	data = map(lambda x: x.rstrip(), data)

	data_json_str = "[" + ','.join(data) + "]"

	return pd.read_json(data_json_str)