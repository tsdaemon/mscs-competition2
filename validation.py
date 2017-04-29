def split_train_test_by_last_record(df):
	df = df.sort_values(by='ts_listen', axis=0)
	by_user = df.groupby(['user_id'], as_index=False).last()
	test = set(by_user['index'])
	train = set(set(train.index) - test)
	return train, test