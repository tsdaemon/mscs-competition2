def minimal_entries_by_user(df, min):
	count_user = df.groupby('user_id', as_index=False).agg({'ts_listen':'count'})
	df = df.join(count_user, on='user_id', rsuffix='_count')
	df = df[df['ts_listen_count'] >= min]
	del df['ts_listen_count']
	del df['user_id_count']
	return df


def remove_percent_entries_for_users_who_have_to_much(df, max, p):
	
	def apply_select_first_p_indexes(group, p):
		l = len(group)
		num = l*p
		return group.index[0:num]
	
	df = df.sort_values(by=['user_id', 'ts_listen'], axis=0)
	count_user = df.groupby('user_id', as_index=False).agg({'ts_listen':'count'})
	df = df.join(count_user, on='user_id', rsuffix='_count')
	
	df_big = df[df['ts_listen_count'] > max]
	to_remove = [i for indexes in df_big.groupby('user_id').apply(lambda group: apply_select_first_p_indexes(group, p)) for i in indexes]
	
	df = df[~df.index.isin(to_remove)]
	
	del df['ts_listen_count']
	del df['user_id_count']
	df = df.sort_index()
	return df
