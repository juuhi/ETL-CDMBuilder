﻿SELECT DISTINCT SOURCE_CODE, TARGET_CONCEPT_ID, VALID_START_DATE, VALID_END_DATE
FROM {sc}.SOURCE_TO_CONCEPT_MAP
WHERE SOURCE_VOCABULARY_ID = 40 AND TARGET_VOCABULARY_ID = 40