import pandas as pd
import numpy as np
import feather
import lightgbm as lgb

def crossvalidate_model(path_to_folds, cols, params, verbose=1, iterations=1000):
    
    evs = []
    for i in range(1,2):
        train = feather.read_dataframe(path_to_folds + "/train_{}.feather".format(i))
        test = feather.read_dataframe(path_to_folds + "/test_{}.feather".format(i))
        lgb_train = lgb.Dataset(train[cols], train["is_listened"])
        lgb_test = lgb.Dataset(test[cols], test["is_listened"])
        evals = {}
        model_lgm = lgb.train(params, lgb_train, iterations, valid_sets=lgb_test, verbose_eval=verbose, evals_result=evals)
        if(verbose):
            print(evals["valid_0"]["auc"][-1])
        evs.append(evals["valid_0"]["auc"][-1])

    return evs


def create_submission(train, test, path_to_sample_submission, params, cols, path_to_submission, verbose=1, iterations=1000):
    lgb_train = lgb.Dataset(train[cols], train["is_listened"])
    model_lgm = lgb.train(params, lgb_train, iterations, verbose_eval=verbose)

    lgbm_preds = model_lgm.predict(test[cols])
    submission = pd.read_csv(path_to_sample_submission)
    submission["is_listened"] = lgbm_preds
    submission.to_csv(path_to_submission, index=False)
    return model_lgm.feature_importance()
    

def feature_score(fs, cols):
    fscore = list(zip(cols, fs))
    fscore.sort(key=lambda x: x[1])
    fscore.reverse()
    return fscore


