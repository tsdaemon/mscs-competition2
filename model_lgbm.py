import pandas as pd
import numpy as np
import feather
import lightgbm as lgb

def crossvalidate_model(path_to_folds, cols, params, verbose=1):
    
    evs = []
    for i in range(1,4):
        train = feather.read_dataframe(path_to_folds + "/train_{}.feather".format(i))
        test = feather.read_dataframe(path_to_folds + "/test_{}.feather".format(i))
        lgb_train = lgb.Dataset(train[cols], train["is_listened"])
        lgb_test = lgb.Dataset(test[cols], test["is_listened"])
        evals = {}
        model_lgm = lgb.train(params, lgb_train, 1000, valid_sets=lgb_test, verbose_eval=verbose, evals_result=evals)
        if(verbose):
            print(evals["valid_0"]["auc"][-1])
        evs.append(evals["valid_0"]["auc"][-1])

    return evs


def create_submission(path_to_train, path_to_test, path_to_sample_submission, params, cols, path_to_submission):
    train = pd.read_csv(path_to_train)
    test = pd.read_csv(path_to_test)
    train = train[train["listen_type"] == 1]

    lgb_train = lgb.Dataset(train[cols], train["is_listened"])
    model_lgm = lgb.train(params, lgb_train, 1000)

    lgbm_preds = model_lgm.predict(test[cols])
    submission = pd.read_csv(path_to_sample_submission)
    submission["is_listened"] = lgbm_preds
    submition.to_csv(path_to_submission, index=False)
    return model_lgm.feature_importance()
    

